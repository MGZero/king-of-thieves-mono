using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using King_of_Thieves.Sound;

namespace King_of_Thieves
{
    public delegate void createHandler(object sender);
    public delegate void destroyHandler(object sender);
    public delegate void keyDownHandler(object sender);
    public delegate void frameHandler(object sender);
    public delegate void drawHandler(object sender);
    public delegate void keyReleaseHandler(object sender);
    public delegate void collideHandler(object sender, Actors.CActor collider);
    public delegate void roomStartHandler(object sender);
    public delegate void roomEndHandler(object sender);
    public delegate void animationEndHandler(object sender);
    public delegate void timerHandler(object sender);
    public delegate void userEventHandler(object sender);
    public delegate void mouseLeftClickHandler(object sender);
    public delegate void clickHandler(object sender);
    public delegate void tapHandler(object sender);
    
}
