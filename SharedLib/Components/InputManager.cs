using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace SharedLib
{
  public class InputManager
  {
    public delegate void PointSelectedHandler(Point point);
    public event PointSelectedHandler PointSelected;

    private TouchCollection _previousTouchCollection;
    private MouseState _previousMouseState;

    public void Initialize()
    {
      if (TouchPanel.IsGestureAvailable)
      {
        TouchPanel.EnabledGestures = GestureType.Tap;
      }
      _previousMouseState = Mouse.GetState();
    }

    public void Update()
    {
      HandleMouse();
      HandleTouch();
    }

    /// <summary>
    /// Handles mouse input and invokes the <see cref="PointSelected"/> event when the left mouse button is pressed.
    /// </summary>
    private void HandleMouse()
    {
      MouseState mouseState = Mouse.GetState();

      if (mouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released)
      {
        PointSelected?.Invoke(new Point(mouseState.X, mouseState.Y));
      }

      _previousMouseState = mouseState;
    }

    /// <summary>
    /// Handles touch input by checking the touch state and invoking the PointSelected event if a touch is detected.
    /// </summary>
    private void HandleTouch()
    {
      TouchCollection touchCollection = TouchPanel.GetState();

      if (touchCollection.Count > 0 && _previousTouchCollection.Count == 0)
      {
        PointSelected?.Invoke(new Point((int)touchCollection[0].Position.X, (int)touchCollection[0].Position.Y));
      }

      _previousTouchCollection = touchCollection;
    }
  }
}