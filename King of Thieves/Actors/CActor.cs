using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using King_of_Thieves.Graphics;
using Gears.Cloud;
using King_of_Thieves.Input;
using System.Timers;
using Gears.Cloud.Utility;

namespace King_of_Thieves.Actors
{
    //Actor states
    //idle: Not doing anything
    public enum ACTORTYPES
    {
        MANAGER = 0,
        INTERACTABLE
    }

    public enum DIRECTION
    {
        UP = 0,
        DOWN,
        LEFT,
        RIGHT
    }

    public enum ACTOR_STATES
    {
        ATTACK = 0,
        CARRY,
        CHASE,
        DAWN,
        DAY,
        DUSK,
        FLYING,
        IDLE,
        KNOCKBACK,
        LIFT,
        MIDNIGHT,
        MORNING,
        MOVING,
        NIGHT,
        POPDOWN,
        POPUP,
        ROLLING,
        SMASH,
        SWINGING,
        SHOCKED,
        THROWING,
        TOSSING,
        WOBBLE
    }

    public abstract class CActor
    {
        public IList<ACTOR_STATES> INVINCIBLE_STATES = new List<ACTOR_STATES>{ACTOR_STATES.KNOCKBACK, ACTOR_STATES.SHOCKED}.AsReadOnly();
        protected Vector2 _position = Vector2.Zero;
        protected Vector2 _oldPosition = Vector2.Zero;
        public readonly ACTORTYPES ACTORTYPE;
        protected string _name;
        protected CAnimation _sprite;
        protected DIRECTION _direction = DIRECTION.UP;
        protected Boolean _moving = false; //used for prioritized movement
        private int _componentAddress = 0;
        protected Dictionary<uint, userEventHandler> _userEvents;
        protected Dictionary<uint, CActor> _userEventsToFire;
        protected ACTOR_STATES _state = ACTOR_STATES.IDLE;
        public Graphics.CSprite image;
        protected Dictionary<string, Graphics.CSprite> _imageIndex;
        protected Dictionary<string, Sound.CSound> _soundIndex;
        private bool _animationHasEnded = false;
        public List<object> userParams = new List<object>();
        public bool _followRoot = true;
        public int layer;
        public CComponent component;
        protected Vector2 _velocity;
        public bool noCollide = false;
        protected string _oldName = ""; //for when moving to different components
        protected CComponent _oldComponent = null;
        protected bool _killMe = false;
        protected int _hp;
        protected bool _enabled = true;
        private string _dataType;

        protected Collision.CHitBox _hitBox;
        protected List<Type> _collidables;
        public static bool showHitBox = false; //Draw hitboxes over actor if this is true


        //event handlers will be added here
        public event createHandler onCreate;
        public event destroyHandler onDestroy;
        public event keyDownHandler onKeyDown;
        public event frameHandler onFrame;
        public event drawHandler onDraw;
        public event keyReleaseHandler onKeyRelease;
        public event collideHandler onCollide;
        public event animationEndHandler onAnimationEnd;
        public event timerHandler onTimer0;
        public event timerHandler onTimer1;
        public event timerHandler onTimer2;
        public event mouseLeftClickHandler onMouseClick;
        public event clickHandler onClick;
        public event tapHandler onTap;

        public virtual void create(object sender) { }
        public virtual void keyDown(object sender) { }
        public virtual void keyRelease(object sender) { }
        public virtual void frame(object sender) { }
        public virtual void draw(object sender) { }
        public virtual void collide(object sender, CActor collider) { }
        public virtual void animationEnd(object sender) { }
        public virtual void timer0(object sender)  { }
        public virtual void timer1(object sender) { }
        public virtual void mouseClick(object sender) { }
        public virtual void click(object sender) { }
        public virtual void tap(object sender) { }
        public virtual void destroy(object sender)
        {
            _hitBox.destroy();
            _hitBox = null;
        }

        protected virtual void applyEffects(){}

        protected virtual void _addCollidables() { } //Use this guy to tell the Actor what kind of actors it can collide with
        protected Random _randNum = new Random();

        private int _timer0 = -1;
        private int _timer1 = -1;
        

        public CActor()
            
        {
            onCreate += new createHandler(create);
            onDestroy += new destroyHandler(destroy);
            onKeyDown += new keyDownHandler(keyDown);
            onKeyRelease += new keyReleaseHandler(keyRelease);
            onFrame += new frameHandler(frame);
            onDraw += new drawHandler(draw);
            onAnimationEnd += new animationEndHandler(animationEnd);
            onCollide += new collideHandler(collide);
            onMouseClick += new mouseLeftClickHandler(mouseClick);
            onTap += new tapHandler(tap);
            onTimer0 += new timerHandler(timer0);
            onTimer1 += new timerHandler(timer1);

            _name = name;
            _collidables = new List<Type>();

            try
            {
                _addCollidables();
            }
            catch (NotImplementedException)
            { ;}

            _position = position;

            try
            {
                onCreate(this);
            }
            catch (NotImplementedException)
            { }

            _registerUserEvents();
            _initializeResources();
        }

        ~CActor()
        {
            onCreate -= new createHandler(create);
            onDestroy -= new destroyHandler(destroy);
            onKeyDown -= new keyDownHandler(keyDown);
            onFrame -= new frameHandler(frame);
            onKeyRelease -= new keyReleaseHandler(keyRelease);
            onDraw -= new drawHandler(draw);
        }

        public string dataType
        {
            get
            {
                return _dataType;
            }
        }

        public string getMapHeaderInfo()
        {
            return _componentAddress + ";" + _name + ";" + dataType + ";" + position.X + ":" + position.Y;
        }

        public void startTimer0(int ticks)
        {
            _timer0 = ticks;
        }

        public void startTimer1(int ticks)
        {
            _timer1 = ticks;
        }

        //overload this and call the base to process your own parameters
        public virtual void init(string name, Vector2 position, string dataType, int compAddress, params string[] additional)
        {
            _name = name;
            _position = position;
            _componentAddress = (int)compAddress;
            _dataType = dataType;
        }

        public Vector2 velocity
        {
            get
            {
                return _velocity;
            }
        }

        public int componentAddress
        {
            get
            {
                return _componentAddress;
            }
        }

        public void setVelocity(float x, float y)
        {
            _velocity.X = x;
            _velocity.Y = y;
        }

        public ACTOR_STATES state
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        public DIRECTION moveToPoint(float x, float y, float speed)
        {
            float distX = 0, distY = 0;

            distX = (x - _position.X);
            distY = (y - _position.Y);

            distX = Math.Sign(distX);
            distY = Math.Sign(distY);

            _position.X += (speed * distX);
            _position.Y += (speed * distY);

            if (distY < 0)
                return DIRECTION.UP;
            else if (distY > 0)
                return DIRECTION.DOWN;

            if (distX < 0)
                return DIRECTION.LEFT;
            else if (distX > 0)
                return DIRECTION.RIGHT;

            return DIRECTION.DOWN;


        }

        public void jumpToPoint(float x, float y)
        {
            _position.X = x;
            _position.Y = y;
        }

        public void swapImage(string imageIndex, bool triggerAnimEnd = true)
        {
            image = _imageIndex[imageIndex];

            if (triggerAnimEnd)
            {
                _animationHasEnded = true;
            }
        }

        public void addFireTrigger(uint userEvent, CActor sender)
        {
            _userEventsToFire.Add(userEvent, sender);
        }

        protected virtual void _registerUserEvents()
        {
            _userEvents = new Dictionary<uint, userEventHandler>();
            _userEventsToFire = new Dictionary<uint, CActor>();
        }

        public DIRECTION direction
        {
            get
            {
                return _direction;
            }
        }

        public CAnimation spriteIndex
        {
            get
            {
                return _sprite;
            }
            set
            {
                _sprite = value;
            }
        }

        public virtual void update(GameTime gameTime)
        {
            //check collisions(This should realy be done after all objects have updated. As it is now two objects can be colliding, be drawn and THEN acted on for their collision)
            if (!_noCollide)
            {
                foreach (Type actor in _collidables)
                {

                    //fetch all actors of this type and check them for collisions
                    CActor[] collideCheck = Map.CMapManager.queryActorRegistry(actor, layer);
                    if (collideCheck == null)
                        continue;

                    foreach (CActor x in collideCheck)
                    {
                        if (!x._noCollide && _hitBox.checkCollision(x))
                        {
                            //trigger collision event
                            onCollide(this, x);
                        }
                    }
                }
            }

            if (_killMe)
            {
                Map.CMapManager.removeFromActorRegistry(this);
                onDestroy(this);
            }
            else
            {
                if (_animationHasEnded)
                    try
                    {
                        onAnimationEnd(this);
                    }
                    catch (NotImplementedException) { ;}


                _oldPosition = _position;

                if (image != null)
                {
                    image.X = (int)_position.X;
                    image.Y = (int)_position.Y;
                }

                if ((Master.GetInputManager().GetCurrentInputHandler() as CInput).areKeysPressed)
                    onKeyDown(this);

                if ((Master.GetInputManager().GetCurrentInputHandler() as CInput).areKeysReleased)
                    onKeyRelease(this);

                if ((Master.GetInputManager().GetCurrentInputHandler() as CInput).mouseLeftClick)
                {
                    onMouseClick(this);


                    if (_hitBox != null && _hitBox.checkCollision(new Vector2((Master.GetInputManager().GetCurrentInputHandler() as CInput).mouseX,
                                                            (Master.GetInputManager().GetCurrentInputHandler() as CInput).mouseY)))
                    {
                        click(this);
                    }
                }

                if ((Master.GetInputManager().GetCurrentInputHandler() as CInput).mouseLeftRelease)
                {
                    onTap(this);
                }

                //new timer stuff THANKS ASH
                if (_timer0 >= 0)
                {
                    _timer0--;

                    if (_timer0 <= 0)
                    {
                        _timer0 = -1;
                        onTimer0(this);
                    }
                }

                if (_timer1 >= 0)
                {
                    _timer1--;

                    if (_timer1 <= 0)
                    {
                        _timer1 = -1;
                        onTimer1(this);
                    }
                }

                foreach (KeyValuePair<uint, CActor> ID in _userEventsToFire)
                {
                    _userEvents[ID.Key](ID.Value);
                }
            }

            _userEventsToFire.Clear();

            _animationHasEnded = false;
            userParams.Clear();

        }

        public virtual void drawMe(bool useOverlay = false)
        {
            onDraw(this);

            //Color overlay = useOverlay ? Controllers.GameControllers.CDayClock.overlay : Color.White;

            if (image != null)
                _animationHasEnded = image.draw((int)_position.X, (int)_position.Y, useOverlay);

            if (showHitBox && _hitBox != null)
                _hitBox.draw();

        }

        public virtual void dealDamange(int damage, CActor target)
        {
            target._hp -= damage;
        }

        private void _dealForce(int force, CActor target)
        {

        }

        public bool isCollidable
        {
            get
            {
                return _hitBox == null;
            }
        }

        protected virtual void _initializeResources()
        {
            //add sprites to image index by overloading this function.
            //also add resources to the texture cache here.
            _imageIndex = new Dictionary<string, CSprite>();
            _soundIndex = new Dictionary<string, Sound.CSound>();
        }

        private void _closeResources()
        {
            _imageIndex.Clear();
            _imageIndex = null;
        }

        public Vector2 position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public Vector2 oldPosition
        {
            get
            {
                return _oldPosition;
            }
            
        }

        public bool killMe
        {
            get
            {
                return _killMe;
            }
        }

        private bool _noCollide
        {
            get
            {
                return _hitBox == null;
            }
        }

        public King_of_Thieves.Actors.Collision.CHitBox hitBox
        {
            get
            {
                return _hitBox;
            }
        }

        public Vector2 distanceFromLastFrame
        {
            get
            {
                return (position - oldPosition);
            }
        }

        public virtual void remove()
        {
            onDestroy(this);
        }

        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public virtual void shock()
        {
            throw new NotImplementedException("You may not call this method from the CActor class. Method: shock()");
        }

        //this will go up to the component and trigger the specified user event in the specified actor
        //what this does is create a "packet" that will float around in some higher level scope for the component to pick up
        protected void _triggerUserEvent(int eventNum, string actorName, params object[] param)
        {
            CMasterControl.commNet[_componentAddress].Add(new CActorPacket(eventNum, actorName, this, param));
        }
    }
}
