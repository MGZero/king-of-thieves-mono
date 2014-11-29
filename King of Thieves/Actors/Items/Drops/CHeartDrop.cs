using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace King_of_Thieves.Actors.Items.Drops
{
    class CHeartDrop : CDroppable
    {
        private static readonly string _HEART = "heart";

        public CHeartDrop() :
            base()
        {
            _capacity = 4;

            _imageIndex.Add(_HEART, new Graphics.CSprite(Graphics.CTextures.DROPS_HEART));
            swapImage(_HEART);
        }

        protected override void _addCollidables()
        {
            base._addCollidables();
            _collidables.Add(typeof(Actors.Player.CPlayer));
        }

        public override void collide(object sender, CActor collider)
        {
            base.collide(sender, collider);

            if (collider is Player.CPlayer)
            {
                _killMe = true;
                CMasterControl.healthController.modifyHp(_capacity);
            }
        }
    }
}
