using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace King_of_Thieves.Actors.Projectiles
{
    class CArrow : CProjectile
    {
        private static int _arrowCount = 0;

        public CArrow(DIRECTION direction, Vector2 velocity, Vector2 position) 
            : base(direction, velocity, position)
        {
            _imageIndex.Add(PROJ_DOWN, new Graphics.CSprite(Graphics.CTextures.EFFECT_ARROW));
            _imageIndex.Add(PROJ_RIGHT, new Graphics.CSprite(Graphics.CTextures.EFFECT_ARROW_RIGHT));
            _imageIndex.Add(PROJ_LEFT, new Graphics.CSprite(Graphics.CTextures.EFFECT_ARROW_RIGHT,true,false));
            _imageIndex.Add(PROJ_UP, new Graphics.CSprite(Graphics.CTextures.EFFECT_ARROW,false,true));

            _arrowCount += 1;
            _name = "arrow" + _arrowCount;
            _hitBox = new Collision.CHitBox(this, 0, 0, 5, 5);
            _damage = 1;

            startTimer1(60);

            switch (_direction)
            {
                case DIRECTION.DOWN:
                    swapImage(PROJ_DOWN);
                    break;

                case DIRECTION.RIGHT:
                    swapImage(PROJ_RIGHT);
                    break;

                case DIRECTION.LEFT:
                    swapImage(PROJ_LEFT);
                    break;

                case DIRECTION.UP:
                    swapImage(PROJ_UP);
                    break;
            }

        }

        public override void collide(object sender, CActor collider)
        {
            base.collide(sender, collider);
            _killMe = true;

            if (collider is Player.CPlayer)
                if (!INVINCIBLE_STATES.Contains(collider.state))
                    collider.dealDamange(_damage, collider);
        }

        public override void destroy(object sender)
        {
            base.destroy(sender);
            _arrowCount -= 1;
        }

        public override void timer1(object sender)
        {
            base.timer1(sender);
            _damage = 3;
        }

        protected override void shoot()
        {
            startTimer0(60);
            _followRoot = false;
            switch (_direction)
            {
                case DIRECTION.DOWN:
                    _velocity.Y = 4;
                    break;

                case DIRECTION.RIGHT:
                    _velocity.X = 4;
                    break;

                case DIRECTION.LEFT:
                    _velocity.X = -4;
                    break;

                case DIRECTION.UP:
                    _velocity.Y = -4;
                    break;
            }
        }

        public void userEventShoot(object sender)
        {
            shoot();
        }

        protected override void _registerUserEvents()
        {
            base._registerUserEvents();

            _userEvents.Add(0, userEventShoot);
        }
        
    }
}
