using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace King_of_Thieves.Actors.NPC.Enemies.Wizzrobe
{
    class CWizzrobe : CBaseWizzrobe
    {
        private static string _SPRITE_NAMESPACE = "Npc:wizzrobe";
        private static string _WIZZROBE_IDLE_DOWN = _SPRITE_NAMESPACE + ":idleDown";
        private static string _WIZZROBE_ATTACK_DOWN = _SPRITE_NAMESPACE + ":attackDown";
        private static string _WIZZROBE_IDLE_LEFT = _SPRITE_NAMESPACE + ":idleLeft";
        private static string _WIZZROBE_ATTACK_LEFT = _SPRITE_NAMESPACE + ":attackLeft";
        private static string _WIZZROBE_IDLE_RIGHT = _SPRITE_NAMESPACE + ":idleRight";
        private static string _WIZZROBE_ATTACK_RIGHT = _SPRITE_NAMESPACE + ":attackRight";
        private static string _WIZZROBE_IDLE_UP = _SPRITE_NAMESPACE + ":idleUp";
        private static string _WIZZROBE_ATTACK_UP = _SPRITE_NAMESPACE + ":attackUp";
        private static int _normalWizzrobeCount = 0;

        public CWizzrobe() :
            base(WIZZROBE_TYPE.NORMAL)
        {
            //we have to cache the sprites
            if (_normalWizzrobeCount <= 0)
            {
                Graphics.CTextures.addTexture(_WIZZROBE_IDLE_DOWN, new Graphics.CTextureAtlas(_NPC_WIZZROBE, 32, 32, 0, "0:0", "0:0"));
                Graphics.CTextures.addTexture(_WIZZROBE_IDLE_LEFT, new Graphics.CTextureAtlas(_NPC_WIZZROBE, 32, 32, 0, "2:0", "2:0"));
                Graphics.CTextures.addTexture(_WIZZROBE_IDLE_UP, new Graphics.CTextureAtlas(_NPC_WIZZROBE, 32, 32, 0, "4:0", "4:0"));

                Graphics.CTextures.addTexture(_WIZZROBE_ATTACK_DOWN, new Graphics.CTextureAtlas(_NPC_WIZZROBE, 32, 32, 0, "1:0", "1:0"));
                Graphics.CTextures.addTexture(_WIZZROBE_ATTACK_LEFT, new Graphics.CTextureAtlas(_NPC_WIZZROBE, 32, 32, 0, "3:0", "3:0"));
                Graphics.CTextures.addTexture(_WIZZROBE_ATTACK_UP, new Graphics.CTextureAtlas(_NPC_WIZZROBE, 32, 32, 0, "5:0", "5:0"));
            }

            _imageIndex.Add(_IDLE_DOWN, new Graphics.CSprite(_WIZZROBE_IDLE_DOWN));
            _imageIndex.Add(_IDLE_LEFT, new Graphics.CSprite(_WIZZROBE_IDLE_LEFT));
            _imageIndex.Add(_IDLE_RIGHT, new Graphics.CSprite(_WIZZROBE_IDLE_LEFT, true));
            _imageIndex.Add(_IDLE_UP, new Graphics.CSprite(_WIZZROBE_IDLE_UP));

            _imageIndex.Add(_ATTACK_DOWN, new Graphics.CSprite(_WIZZROBE_ATTACK_DOWN));
            _imageIndex.Add(_ATTACK_LEFT, new Graphics.CSprite(_WIZZROBE_ATTACK_LEFT));
            _imageIndex.Add(_ATTACK_RIGHT, new Graphics.CSprite(_WIZZROBE_ATTACK_LEFT, true));
            _imageIndex.Add(_ATTACK_UP, new Graphics.CSprite(_WIZZROBE_ATTACK_UP));

            _normalWizzrobeCount += 1;
        }

        protected override void cleanUp()
        {
            if (_normalWizzrobeCount <= 0)
            {
                Graphics.CTextures.cleanUp(_SPRITE_NAMESPACE);
                _normalWizzrobeCount = 0;
                base.cleanUp();
            }
            
        }

        public override void destroy(object sender)
        {
            _normalWizzrobeCount--;

            if (_normalWizzrobeCount <= 0)
            {
                cleanUp();
                _normalWizzrobeCount = 0;
            }

            base.destroy(sender);
        }


    }
}
