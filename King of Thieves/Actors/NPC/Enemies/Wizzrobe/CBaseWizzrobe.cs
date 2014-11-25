using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
        protected readonly static string _NPC_WIZZROBE = "npc:wizzrobe";
        private static int _wizzrobeCount = 0;

        protected readonly static string _IDLE_DOWN = "idleDown";
        protected readonly static string _ATTACK_DOWN = "attackDown";
        protected readonly static string _IDLE_LEFT = "idleLeft";
        protected readonly static string _ATTACK_LEFT = "attackLeft";
        protected readonly static string _IDLE_RIGHT = "idleRight";
        protected readonly static string _ATTACK_RIGHT = "attackRight";
        protected readonly static string _IDLE_UP = "idleUp";
        protected readonly static string _ATTACK_UP = "attackUp";

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

        }

        public override void create(object sender)
        {
            _vanish(false);
            base.create(sender);
        }

        public override void destroy(object sender)
        {
            _wizzrobeCount--;

            if (_wizzrobeCount <= 0)
            {
                cleanUp();
                _wizzrobeCount = 0;
            }

            base.destroy(sender);
        }

        protected override void cleanUp()
        {
            Graphics.CTextures.rawTextures.Remove(_NPC_WIZZROBE);
        }

        public override void timer0(object sender)
        {
            base.timer0(sender);

            _vanish();
        }

        public override void timer1(object sender)
        {
            base.timer1(sender);
            _appear();
        }

        public override void timer2(object sender)
        {
            base.timer2(sender);
            _attack();
        }

        private void _appear()
        {
            Graphics.CEffects.createEffect(Graphics.CEffects.SMOKE_POOF, new Vector2(_position.X - 13, _position.Y - 5));
            _state = ACTOR_STATES.IDLE;
            startTimer0(_IDLE_TIME);

            switch (_direction)
            {
                case DIRECTION.DOWN:
                    swapImage(_IDLE_DOWN);
                    break;

                case DIRECTION.UP:
                    swapImage(_IDLE_UP);
                    break;

                case DIRECTION.LEFT:
                    swapImage(_IDLE_LEFT);
                    break;

                case DIRECTION.RIGHT:
                    swapImage(_IDLE_RIGHT);
                    break;
            }
        }

        private void _vanish(bool showEffect = true)
        {
            if (showEffect)
                Graphics.CEffects.createEffect(Graphics.CEffects.SMOKE_POOF, new Vector2(_position.X - 13, _position.Y - 5));

            _state = ACTOR_STATES.INVISIBLE;

            Random rand = new Random();
            startTimer1(_VANISH_TIME[rand.Next(2)]);
            rand = null;
        }

        private void _attack()
        {
            switch (_direction)
            {

            }
        }

        public override void drawMe(bool useOverlay = false)
        {
            if (_state != ACTOR_STATES.INVISIBLE)
                base.drawMe(useOverlay);
        }

    }
}
