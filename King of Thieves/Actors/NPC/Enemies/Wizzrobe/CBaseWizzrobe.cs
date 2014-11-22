using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace King_of_Thieves.Actors.NPC.Enemies.Wizzrobe
{
    enum WIZZROBE_TYPE
    {
        NORMAL = 0,
        FIRE,
        ICE,
        LIGHTNING
    }

    class CBaseWizzrobe : CBaseEnemy
    {
        protected readonly WIZZROBE_TYPE _type;
        private const int _IDLE_TIME = 240; //the time between appearing and attacking/dissapearing
        private readonly int[] _VANISH_TIME = {240,180,300}; //the time they are invisible for
        private const int _ATTACK_TIME = 60; //the time for playing the attack frames
        private const string _NPC_WIZZROBE = "npc:wizzrobe";
        private static int _wizzrobeCount = 0;

        private static string _WIZZROBE_IDLE_DOWN = "wizzrobe:idleDown";
        private static string _WIZZROBE_ATTACK_DOWN = "wizzrobe:attackDown";
        private static string _WIZZROBE_IDLE_LEFT = "wizzrobe:idleLeft";
        private static string _WIZZROBE_ATTACK_LEFT = "wizzrobe:attackLeft";
        private static string _WIZZROBE_IDLE_RIGHT = "wizzrobe:idleRight";
        private static string _WIZZROBE_ATTACK_RIGHT = "wizzrobe:attackRight";
        private static string _WIZZROBE_IDLE_UP = "wizzrobe:idleUp";
        private static string _WIZZROBE_ATTACK_UP = "wizzrobe:attackUp";

        public CBaseWizzrobe(WIZZROBE_TYPE type, params dropRate[] drops)
            : base(drops)
        {
            _type = type;
            //cache the textures needed
            if (!Graphics.CTextures.rawTextures.ContainsKey(_NPC_WIZZROBE))
                Graphics.CTextures.rawTextures.Add(_NPC_WIZZROBE, CMasterControl.glblContent.Load<Texture2D>(@"sprites/npc/wizzrobe"));

            _wizzrobeCount += 1;
            _direction = DIRECTION.DOWN;
        }

        public override void update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.update(gameTime);

            switch (_direction)
            {
                case DIRECTION.DOWN:
                    break;
            }
        }

        public override void destroy(object sender)
        {
            _wizzrobeCount--;

            if (_wizzrobeCount <= 0)
                Graphics.CTextures.rawTextures.Remove(_NPC_WIZZROBE);

            base.destroy(sender);
        }

        public override void timer0(object sender)
        {
            base.timer0(sender);

            //go into attack state
            startTimer2(_ATTACK_TIME);
            _state = ACTOR_STATES.ATTACK;
        }

        public override void timer2(object sender)
        {
            base.timer2(sender);

            _attack();
        }

        private void _appear()
        {
            Graphics.CEffects.createEffect(Graphics.CEffects.SMOKE_POOF, _position);
            _state = ACTOR_STATES.IDLE;
            startTimer0(_IDLE_TIME);

            switch (_direction)
            {
                case DIRECTION.DOWN:
                    swapImage(_WIZZROBE_IDLE_DOWN);
                    break;
            }
        }

        private void _vanish()
        {
            Graphics.CEffects.createEffect(Graphics.CEffects.SMOKE_POOF, _position);
            _state = ACTOR_STATES.INVISIBLE;

            Random rand = new Random();
            startTimer1(_VANISH_TIME[rand.Next(2)]);
            rand = null;
        }

        private void _attack()
        {
            _vanish();
        }

        public override void drawMe(bool useOverlay = false)
        {
            if (_state != ACTOR_STATES.INVISIBLE)
                base.drawMe(useOverlay);
        }

    }
}
