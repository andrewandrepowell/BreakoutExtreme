using BreakoutExtreme.Utility;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System.Diagnostics;
using System;
using MonoGame.Extended.Collections;
using System.Xml.Linq;


namespace BreakoutExtreme.Components
{
    public partial class Ball
    {
        private class Launcher(Ball parent)
        {
            private readonly static Vector2 _defaultAcceleration = new(0, -5000);
            private const float _horizontalLimitDegrees = 30;
            private readonly static Vector2 _horizontalLimitVector = new(
                (float)Math.Cos(_horizontalLimitDegrees / 180 * Math.PI),
                (float)Math.Sin(_horizontalLimitDegrees / 180 * Math.PI));
            private readonly static Vector2 _horizontaLimitVectorLower = new(
                _horizontalLimitVector.Y, 
                -_horizontalLimitVector.X);
            private readonly static Vector2 _horizontaLimitVectorUpper = new(
                -_horizontalLimitVector.Y,
                _horizontalLimitVector.X);
            private readonly Ball _parent = parent;
            private readonly Deque<Brick> _powerBricks = [];
            private readonly Deque<Collider.CollideNode> _nodes = [];
            private Vector2 _acceleration = new(0, -5000);
            public bool Running { get; private set; } = false;
            public Vector2 Acceleration
            {
                get => _acceleration;
                set
                {
                    Debug.Assert(!value.EqualsWithTolerence(Vector2.Zero));
                    _acceleration = value;
                }
            }
            public float Magnitude
            {
                get => Acceleration.Length();
                set
                {
                    Debug.Assert(value > 0);
                    Acceleration = Acceleration.NormalizedCopy() * value;
                }
            }
            public static void ServiceApplyDamage(
                Collider.CollideNode node, 
                PlayArea playArea, 
                Deque<Brick> powerBricks)
            {
                // Handle damaging a brick logic.
                {
                    if (node.Other.Parent is Brick brick && brick.State == Brick.States.Active)
                    {
                        brick.Damage();

                        // Perform actions if brick is destroyed.
                        if (brick.State != Brick.States.Active)
                        {
                            // Update score.
                            playArea.UpdateScore(brick);

                            // Create power if power bricks.
                            Debug.Assert(powerBricks.Count < 8);
                            if (brick.GetBrick() == Brick.Bricks.Power)
                                powerBricks.AddToBack(brick);
                        }
                    }
                }

                // Handle damaging a cannon logic.
                {
                    if (node.Other.Parent is Cannon cannon && cannon.State == Cannon.States.Active)
                    {
                        cannon.Damage();

                        // Update the score once the cannon is destroyed.
                        if (cannon.State == Cannon.States.Destroying)
                            playArea.UpdateScore(cannon);
                    }
                }
            }
            public static void ServicePowerBricks(Deque<Brick> powerBricks, PlayArea playArea, Collider collider)
            {
                
                while (powerBricks.RemoveFromFront(out var brick))
                {
                    Debug.Assert(brick.GetBrick() == Brick.Bricks.Power);
                    Debug.Assert(brick.Power.HasValue);
                    var brickCollider = brick.GetCollider();
                    switch (brick.Power.Value)
                    {
                        case Powers.MultiBall:
                            {
                                var ball = playArea.CreateBall();
                                var ballCollider = ball.GetCollider();
                                var acceleration = (Vector2?)(collider.Acceleration.EqualsWithTolerence(Vector2.Zero) ? null : -collider.Acceleration);
                                ballCollider.Position = brickCollider.Position + (Vector2)(brickCollider.Size / 2);
                                ball.StartLaunch(acceleration);
                                ball.Spawn();
                            }
                            break;
                        case Powers.Empowered:
                        case Powers.EnlargePaddle:
                        case Powers.NewBall:
                        case Powers.Protection:
                            {
                                var power = Globals.Runner.CreatePower(brick.Power.Value, playArea);
                                power.GetCollider().Position = brickCollider.Position;
                            }
                            break;
                    }
                } 
            }
            public void ServiceCollision(Collider.CollideNode node)
            {
                Debug.Assert(_parent._initialized);
                Debug.Assert(_parent.State == States.Active);
                var collider = _parent.GetCollider();

                _nodes.AddToBack(node);
            }
            public void Start(Vector2? acceleration = null)
            {
                Debug.Assert(_parent._initialized);
                Debug.Assert(_parent._state == States.Active);
                Debug.Assert(!_parent._particler.Running);
                Debug.Assert(!Running);
                _parent._particler.Start();
                _parent._sounder.Play(Sounder.Sounds.Launch);
                if (acceleration.HasValue)
                    Acceleration = acceleration.Value;
                else
                    Acceleration = _defaultAcceleration;
                Debug.Assert(!Acceleration.EqualsWithTolerence(Vector2.Zero));
                Running = true;
            }
            public void Start(float magnitude)
            {
                Start();
                Magnitude = magnitude;
            }
            public void Stop()
            {
                Debug.Assert(_parent._initialized);
                Debug.Assert(_parent._state == States.Active);
                Debug.Assert(_parent._particler.Running);
                Debug.Assert(Running);
                var collider = _parent._collider;
                collider.Acceleration = Vector2.Zero;
                collider.Velocity = Vector2.Zero;
                _parent._particler.Stop();
                _powerBricks.Clear();
                Running = false;
            }
            public void Update()
            {
                if (Running)
                {
                    _parent._collider.Acceleration += Acceleration;
                }

                {
                    var collider = _parent._collider;
                    while (_nodes.RemoveFromFront(out var node))
                    {
                        // Handle rectangular bounce logic.
                        {
                            if (_parent.State == States.Active &&
                                (node.Other.Parent is Wall ||
                                 node.Other.Parent is Paddle ||
                                (node.Other.Parent is Brick brick && brick.State == Brick.States.Active) ||
                                (node.Other.Parent is DeathWall deathWall && deathWall.State == DeathWall.States.Protecting)))
                            {
                                var otherBounds = node.Other.Bounds.BoundingRectangle;
                                var otherPosition = otherBounds.Center;
                                var position = collider.Position;
                                var xBounce = position.X >= otherBounds.Right || position.X <= otherBounds.Left;
                                var yBounce = position.Y >= otherBounds.Bottom || position.Y <= otherBounds.Top;
                                var directions = new Vector2(
                                    Math.Sign(xBounce ? position.X - otherPosition.X : collider.Velocity.X),
                                    Math.Sign(yBounce ? position.Y - otherPosition.Y : collider.Velocity.Y));

                                if (Running)
                                    Acceleration = new Vector2(
                                        directions.X * Math.Abs(Acceleration.X),
                                        directions.Y * Math.Abs(Acceleration.Y));
                                collider.Velocity = new Vector2(
                                        directions.X * Math.Abs(collider.Velocity.X),
                                        directions.Y * Math.Abs(collider.Velocity.Y));
                            }
                        }

                        // Handle circular bounce logic.
                        {
                            var cannon = node.Other.Parent as Cannon;
                            var ball = node.Other.Parent as Ball;
                            if (_parent.State == States.Active && (
                                (cannon != null && cannon.State == Cannon.States.Active) ||
                                (ball != null && ball.State == States.Active)))
                            {
                                var circularCollider =
                                    (cannon != null) ? cannon.GetCollider() :
                                    (ball != null) ? ball.GetCollider() : null;
                                var normalBasis = Vector2.Normalize(collider.Position - circularCollider.Position);
                                var orthogBasis = new Vector2(x: -normalBasis.Y, y: normalBasis.X);
                                {
                                    var normalMag = normalBasis.Dot(collider.Velocity);
                                    var orthogMag = orthogBasis.Dot(collider.Velocity);
                                    collider.Velocity = -normalMag * normalBasis + orthogMag * orthogBasis;
                                }
                                if (Running)
                                {
                                    var normalMag = normalBasis.Dot(Acceleration);
                                    var orthogMag = orthogBasis.Dot(Acceleration);
                                    Acceleration = -normalMag * normalBasis + orthogMag * orthogBasis;
                                }
                            }
                        }

                        // Handle paddle bounce adjustment logic.
                        {
                            if (_parent.State == States.Active && node.Other.Parent is Paddle paddle)
                            {
                                if (Running && !Acceleration.EqualsWithTolerence(Vector2.Zero) && !collider.Velocity.EqualsWithTolerence(Vector2.Zero) && Acceleration.Y < 0)
                                {
                                    var accelerationMagnitude = Acceleration.Length();
                                    var accelerationDirection = Acceleration / accelerationMagnitude;
                                    var accelerationRads = Math.Atan2(-accelerationDirection.Y, accelerationDirection.X);
                                    var velocityMagitude = collider.Velocity.Length();
                                    var paddleX = node.Other.Position.X + node.Other.Size.Width / 2;
                                    var ballX = collider.Position.X;

                                    var divCoef = MathHelper.Pi * 0.25f;
                                    var mapCoef = MathHelper.Clamp((ballX - paddleX) / node.Other.Size.Width, -.5f, .5f) + 0.5f;
                                    var radCoef = MathHelper.Lerp(MathHelper.Pi - divCoef, divCoef, mapCoef);

                                    var newRads = (radCoef + accelerationRads) / 2;
                                    var newDirection = new Vector2(x: (float)Math.Cos(newRads), y: (float)-Math.Sin(newRads));
                                    var newAcceleration = accelerationMagnitude * newDirection;
                                    var newVelocity = velocityMagitude * newDirection;
                                    Acceleration = newAcceleration;
                                    collider.Velocity = newVelocity;
                                }
                            }
                        }

                        // Handle horizontal correction logic.
                        {
                            if (_parent.State == States.Active && Running)
                            {
                                var accelerationMagnitude = Acceleration.Length();
                                var accelerationDirection = Acceleration / accelerationMagnitude;
                                var accelerationSign = new Vector2(Math.Sign(accelerationDirection.X), Math.Sign(accelerationDirection.Y));
                                var accelerationPosDirection = accelerationDirection * accelerationSign;
                                var correctionRequired = accelerationPosDirection.Dot(_horizontaLimitVectorLower) > accelerationPosDirection.Dot(_horizontaLimitVectorUpper);
                                if (correctionRequired)
                                {
                                    var velocityMagitude = collider.Velocity.Length();
                                    var newDirection = _horizontalLimitVector * accelerationSign;
                                    var newAcceleration = newDirection * accelerationMagnitude;
                                    var newVelocity = newDirection * velocityMagitude;
                                    Acceleration = _horizontalLimitVector * accelerationSign * accelerationMagnitude;
                                    collider.Velocity = newVelocity;
                                }
                            }
                        }

                        // Handle collisions related to applying damage.
                        if (_parent.State == States.Active)
                        {
                            ServiceApplyDamage(
                                node: node,
                                playArea: _parent._parent,
                                powerBricks: _powerBricks);
                        }

                        // Handle collision with denotating bomb.
                        {
                            if (_parent.State == States.Active &&
                                node.Other.Parent is Bomb bomb && bomb.State == Bomb.States.Detonating)
                            {
                                _parent.Destroy();
                            }
                        }

                        // Handle collision with death wall.
                        {
                            if (_parent.State == States.Active &&
                                node.Other.Parent is DeathWall deathWall && deathWall.State == DeathWall.States.Active)
                            {
                                _parent.Destroy();
                            }
                        }

                        // Play bounce effects depending on what the ball collided with.
                        {
                            {
                                if (_parent.State == States.Active &&
                                    node.Other.Parent is DeathWall deathWall &&
                                    deathWall.State == DeathWall.States.Protecting)
                                {
                                    deathWall.RunBounceEffects();
                                }
                            }
                            {
                                if (_parent.State == States.Active && node.Other.Parent is Paddle paddle &&
                                    paddle.State == Paddle.States.Active)
                                {
                                    paddle.RunBounceEffects();
                                }
                            }
                            {
                                if (_parent.State == States.Active && node.Other.Parent is Wall wall)
                                {
                                    wall.RunBounceEffects();
                                }
                            }
                            {
                                if (_parent.State == States.Active &&
                                    node.Other.Parent is Ball ball && ball.State == States.Active)
                                {
                                    ball.RunBounceEffects();
                                }
                            }
                        }
                    }
                }

                // Generate powers upon destruction of a power brick.
                ServicePowerBricks(
                    powerBricks: _powerBricks, 
                    playArea: _parent._parent, 
                    collider: _parent._collider);
            }
        }
    }
}
