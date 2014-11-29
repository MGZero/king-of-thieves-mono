﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace King_of_Thieves.Actors.NPC.Enemies.Wizzrobe
{
    class CIceWizzrobe : CBaseWizzrobe
    {
        private static int _iceWizzrobeCount = 0;
        //texture atlas constants
        protected readonly static string _SPRITE_NAMESPACE = "Npc:IceWizzrobe";
        protected readonly static string _WIZZROBE_IDLE_DOWN = _SPRITE_NAMESPACE + ":idleDown";
        protected readonly static string _WIZZROBE_ATTACK_DOWN = _SPRITE_NAMESPACE + ":attackDown";
        protected readonly static string _WIZZROBE_IDLE_LEFT = _SPRITE_NAMESPACE + ":idleLeft";
        protected readonly static string _WIZZROBE_ATTACK_LEFT = _SPRITE_NAMESPACE + ":attackLeft";
        protected readonly static string _WIZZROBE_IDLE_RIGHT = _SPRITE_NAMESPACE + ":idleRight";
        protected readonly static string _WIZZROBE_ATTACK_RIGHT = _SPRITE_NAMESPACE + ":attackRight";
        protected readonly static string _WIZZROBE_IDLE_UP = _SPRITE_NAMESPACE + ":idleUp";
        protected readonly static string _WIZZROBE_ATTACK_UP = _SPRITE_NAMESPACE + ":attackUp";

        public CIceWizzrobe() :
            base(WIZZROBE_TYPE.ICE)
        {
            //we have to cache the sprites
            if (_iceWizzrobeCount <= 0)
            {
                Graphics.CTextures.addTexture(_WIZZROBE_IDLE_DOWN, new Graphics.CTextureAtlas(_NPC_WIZZROBE, 32, 32, 0, "0:3", "0:3"));
                Graphics.CTextures.addTexture(_WIZZROBE_IDLE_LEFT, new Graphics.CTextureAtlas(_NPC_WIZZROBE, 32, 32, 0, "2:3", "2:3"));
                Graphics.CTextures.addTexture(_WIZZROBE_IDLE_UP, new Graphics.CTextureAtlas(_NPC_WIZZROBE, 32, 32, 0, "4:3", "4:3"));

                Graphics.CTextures.addTexture(_WIZZROBE_ATTACK_DOWN, new Graphics.CTextureAtlas(_NPC_WIZZROBE, 32, 32, 0, "1:3", "1:3"));
                Graphics.CTextures.addTexture(_WIZZROBE_ATTACK_LEFT, new Graphics.CTextureAtlas(_NPC_WIZZROBE, 32, 32, 0, "3:3", "3:3"));
                Graphics.CTextures.addTexture(_WIZZROBE_ATTACK_UP, new Graphics.CTextureAtlas(_NPC_WIZZROBE, 32, 32, 0, "5:3", "5:3"));
            }

            _imageIndex.Add(_IDLE_DOWN, new Graphics.CSprite(_WIZZROBE_IDLE_DOWN));
            _imageIndex.Add(_IDLE_LEFT, new Graphics.CSprite(_WIZZROBE_IDLE_LEFT));
            _imageIndex.Add(_IDLE_RIGHT, new Graphics.CSprite(_WIZZROBE_IDLE_LEFT, true));
            _imageIndex.Add(_IDLE_UP, new Graphics.CSprite(_WIZZROBE_IDLE_UP));

            _imageIndex.Add(_ATTACK_DOWN, new Graphics.CSprite(_WIZZROBE_ATTACK_DOWN));
            _imageIndex.Add(_ATTACK_LEFT, new Graphics.CSprite(_WIZZROBE_ATTACK_LEFT));
            _imageIndex.Add(_ATTACK_RIGHT, new Graphics.CSprite(_WIZZROBE_ATTACK_LEFT, true));
            _imageIndex.Add(_ATTACK_UP, new Graphics.CSprite(_WIZZROBE_ATTACK_UP));

            _iceWizzrobeCount += 1;
        }

        protected override void cleanUp()
        {
            if (_iceWizzrobeCount <= 0)
            {
                Graphics.CTextures.cleanUp(_SPRITE_NAMESPACE);
                _iceWizzrobeCount = 0;
                base.cleanUp();
            }

        }

        public override void destroy(object sender)
        {
            _iceWizzrobeCount--;

            if (_iceWizzrobeCount <= 0)
            {
                cleanUp();
                _iceWizzrobeCount = 0;
            }

            base.destroy(sender);
        }

        protected override void _fireProjectile()
        {
            Vector2 projectileVelo = Vector2.Zero;

            switch (_direction)
            {
                case DIRECTION.DOWN:
                    projectileVelo.Y = 5;
                    break;

                case DIRECTION.UP:
                    projectileVelo.Y = -5;
                    break;

                case DIRECTION.RIGHT:
                    projectileVelo.X = 5;
                    break;

                case DIRECTION.LEFT:
                    projectileVelo.X = -5;
                    break;
            }
            Map.CMapManager.addActorToComponent(new Actors.Projectiles.CIceBall(_direction, projectileVelo, _position), componentAddress);
        }
    }
}
