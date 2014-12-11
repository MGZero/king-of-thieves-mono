﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using King_of_Thieves.Graphics;
using Gears.Cloud;
using King_of_Thieves.Input;
namespace King_of_Thieves.Actors.NPC.Enemies
{
    //has all the things that an enemy NPC will have
    //ex: item drops
    public class dropRate
    {
        public dropRate(Items.Drops.CDroppable drop, float rate)
        {
            item = drop;
            this.rate = rate;
        }

        public readonly Items.Drops.CDroppable item;
        public readonly float rate;
    }

    public enum ENEMY_PROPERTIES
    {
        ELECTRIC = 0,
        FIRE,
        ICE
    }

    public abstract class CBaseEnemy : CActor
    {
        protected Dictionary<Items.Drops.CDroppable, float> _itemDrop = new Dictionary<Items.Drops.CDroppable,float>();
        protected int _lineOfSight;
        protected int _fovMagnitude;
        protected float _visionRange; //this is an angle
        protected float _visionSlope;
        protected int _hearingRadius; //how far away they can hear you from
        protected bool _huntPlayer = false;
        protected List<ENEMY_PROPERTIES> _properties = new List<ENEMY_PROPERTIES>();

        public CBaseEnemy(params dropRate[] drops) 
            :  base()
        {
            float sum = 0;
            foreach (dropRate x in drops)
            {
                //this should add up to 1
                sum += x.rate;
                _itemDrop.Add(x.item, sum);
            }

            //calculate field of view
            _fovMagnitude = (int)Math.Cos(_visionRange * (Math.PI / 180.0));
            _visionSlope = (int)Math.Tan(_visionRange * (Math.PI/180.0));
        }

        protected override void _initializeResources()
        {
            base._initializeResources();
        }

        public bool hasProperty(ENEMY_PROPERTIES property)
        {
            return _properties.Contains(property);
        }

        //just chill there
        protected virtual void idle()
        {
            _huntPlayer = hunt();
        }

        //look for the player while idling
        protected bool hunt()
        {
            //check if the player is within the line of sight
            //switch (_direction)
            //{
            //    case DIRECTION.UP:
            //        if (Actors.Player.CPlayer.glblY <= _position.Y && Actors.Player.CPlayer.glblY >= (_position.Y - _lineOfSight))
            //        {

            //        }
            //        break;
            //}
            
            //check hearing field
            if (MathExt.MathExt.distance(_position, new Vector2(Player.CPlayer.glblX, Player.CPlayer.glblY)) <= _hearingRadius)
                return true;

            return false;

        }

        //chase the player
        protected virtual void chase()
        {

        }

        public override void destroy(object sender)
        {
            Items.Drops.CDroppable itemToDrop = _dropItem();
            Map.CMapManager.addActorToComponent(itemToDrop, CReservedAddresses.DROP_CONTROLLER);

            if (_hitBox != null)
                base.destroy(sender);
        }

        private Items.Drops.CDroppable _dropItem()
        {
            Random roller = new Random();
            
            //pick a random number and see which range it falls into
            double selection = _randNum.NextDouble() * 100;
            float previous = 0;

            foreach (KeyValuePair<Items.Drops.CDroppable, float> x in _itemDrop)
            {
                if (selection >= 0 && selection <= x.Value - previous)
                    return x.Key;

                previous = x.Value;
            }

            return null;
            
        }

        protected bool _checkLineofSight(float x, float y)
        {
            //return _visionSlope * (x - _position.X) + (y - _position.Y);

            switch (_direction)
            {
                case DIRECTION.UP:
                    //return 
                    break;
            }


            return false;
        }
    }
}
