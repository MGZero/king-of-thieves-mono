﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace King_of_Thieves.Actors
{
    //component is a series of actors.  Bongo Bongo for example would be 3 actors: 2 hands and a main body, main body being the root.
    //this is really a wrapper around CActor to group related CActors together.
    public class CComponent : Gears.Playable.Unit
    {
        //if the root moves, all children follow it.  Actors otherwise are free to move freely of each other.
        //actors can also rotate around the root.
        public CActor root;
        public uint rootDrawHeight; //The height that root is to draw at, that is, how many elements to draw behind root
        public Dictionary<string, CActor> actors;
        private int _address;
        private uint currentDrawHeight;
        public bool enabled = true;
        private List<CActor> _removeThese = new List<CActor>();
        private bool _killMe = false;
        public int layer = 0;
        public bool useDrawOverlay = true;

        public object getProperty(string actorName, Map.EActorProperties property)
        {
            if (!actors.Keys.Contains(actorName))
            {
                if (root.name == actorName)
                {
                    return propertySwitch(root, property);
                }
                return null;
            }

            CActor actor = actors[actorName];
            return propertySwitch(actor, property);
            
        }

        public List<string> prepareActorHeaderInfo()
        {
            List<string> output = new List<string>();
            foreach (CActor actor in actors.Values)
            {
                output.Add(actor.name + ";" + actor.dataType + ";" + actor.position.X + ":" + actor.position.Y);
            }
            return output;
        }

        private object propertySwitch(CActor actor, Map.EActorProperties property)
        {
            switch (property)
            {
                case Map.EActorProperties.POSITION:
                    return actor.position;

                default:
                    return null;
            }
        }

        protected override string TextureFileLocation
        {
            get { return ""; }
        }

        public CComponent(int address = 0)
        {
            actors = new Dictionary<string, CActor>();
            _address = address;
        }

        public bool killMe
        {
            get
            {
                return _killMe;
            }
        }

        public int address
        {
            get
            {
                return _address;
            }
        }

        private void passMessage(ref CActor actor, CActor sender, uint eventID, params object[] param)
        {
            actor.addFireTrigger(eventID, sender);
            actor.userParams.AddRange(param);
        }

        private void _checkCommNet(string type, CActor actor)
        {
            if (CMasterControl.commNet[(int)_address].Count() > 0)
            {
                CActorPacket[] packetData = new CActorPacket[CMasterControl.commNet[(int)_address].Count()];
                CMasterControl.commNet[(int)_address].CopyTo(packetData);

                var group = from packets in packetData
                            where type == packets.actor
                            select packets;

                foreach (var result in group)
                {
                    //pass the message to the actor
                    CActor temp = actor;
                    passMessage(ref temp, result.sender, (uint)result.userEventID, result.getParams());
                    CMasterControl.commNet[(int)_address].Remove(result);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {

            if (root.killMe)
            {
                _destroyActors();
                enabled = false;
            }

            if (enabled)
            {
                root.update(gameTime);

                foreach (KeyValuePair<string, CActor> kvp in actors)
                {
                    if (kvp.Value.killMe)
                    {
                        removeActor(kvp.Value, true);
                        continue;
                    }

                    //first get messages from the commNet
                    _checkCommNet(kvp.Key, kvp.Value);

                    //update position relative to the root
                    if (kvp.Value._followRoot)
                        kvp.Value.position += root.distanceFromLastFrame;

                    //update
                    kvp.Value.update(gameTime);
                }
                //remove any actors that are to be removed
                for (int i = 0; i < _removeThese.Count(); i++)
                    removeActor(_removeThese[i]);

                _removeThese.Clear();
            }
        }

        public override void Draw(SpriteBatch spriteBatch) //spritebatch not used
        {
            currentDrawHeight = 0;
            if (!this.killMe)
            {
                foreach (KeyValuePair<string, CActor> kvp in actors)
                {
                    if (!kvp.Value.killMe)
                    {
                        if (!root.killMe && rootDrawHeight == currentDrawHeight++)
                            root.drawMe(useDrawOverlay);
                        kvp.Value.drawMe(useDrawOverlay);
                    }
                }
                //If root is last
                if (root != null && !root.killMe && rootDrawHeight == currentDrawHeight)
                    root.drawMe(useDrawOverlay);
            }
        }

        private void _destroyActors()
        {
            root.remove();
            root = null;


            foreach (KeyValuePair<string, CActor> kvp in actors)
            {
                kvp.Value.remove();
                Map.CMapManager.removeFromActorRegistry(kvp.Value);
            }
            actors.Clear();
            Map.CMapManager.removeComponent(this);
        }

        public int getAddress()
        {
            return _address;
        }

        public void addActor(CActor actor, String name)
        {
            //Add as root if no root is set
            if (root != null)
                actors.Add(name, actor);
            else
            {
                layer = actor.layer;
                root = actor;
            }
            //Allow actor to access it's component
            actor.component = this;
        }

        public void removeActor(CActor actor, bool nextCycle = false)
        {
            if (!nextCycle)
            {
                if (actor == root)
                {
                    _killMe = true;
                    enabled = false;
                }
                else
                {
                    try
                    {
                        actors.Remove(actor.name);
                        actor.component = null;
                    }
                    catch (KeyNotFoundException) { }
                }
            }
            else
                _removeThese.Add(actor);
        }
    }
}
