﻿using System;

namespace ConsoleRPG
{
    public class Player : BaseObject
    {


        Renderer renderer;
        Position position;
        Movement movement;
        Collider collider;
        Health health;

        AimingIndicator aimingIndicator;
        Movement.Direction direction;

        public static bool test;
        public static bool turnOffTest;

        float testStrength = 20;

        //Starts false.   Player clicks shoot button. Starts aiming(true). Then clicks again then fireball fires
        public bool fireballAim;
        public bool fireballAimSwitch;
        Cooldown fireballAimFlashCooldown;

        Color baseColor;
        Color fireballAimFlashColor1;
        Color fireballAimFlashColor2;

        float fireballStrength = 20;
        float fireballSpeed = 100;
        int freezeStrength = 5;

        public Player(Map map, int x, int y) : base(map)
        {

            baseColor = new Color(92, 255, 133, 0.99f);
            fireballAimFlashColor1 = new Color(255, 100, 50, 0.99f);
            fireballAimFlashColor2 = new Color(200, 50, 50, 0.99f);

            health = new Health(this, 10, OnDeath);
            //☺
            //Θ
            renderer = new Renderer(this, '☺', 3, baseColor, false);
            position = new Position(this, x, y);
            movement = new Movement(this);
            collider = new Collider(this, OnCollision);
            fireballAimFlashCooldown = new Cooldown(100);

            GetMap().addingObjecsFuncs.Add(SpawnIndicator);

        }

        public void LoadInput()
        {

            //Movement
            InputHandler.AddListener(new KeyListener(MoveUp, ConsoleKey.W));
            InputHandler.AddListener(new KeyListener(MoveDown, ConsoleKey.S));
            InputHandler.AddListener(new KeyListener(MoveLeft, ConsoleKey.A));
            InputHandler.AddListener(new KeyListener(MoveRight, ConsoleKey.D));

            //Aiming
            InputHandler.AddListener(new KeyListener(AimUp, ConsoleKey.UpArrow));
            InputHandler.AddListener(new KeyListener(AimDown, ConsoleKey.DownArrow));
            InputHandler.AddListener(new KeyListener(AimLeft, ConsoleKey.LeftArrow));
            InputHandler.AddListener(new KeyListener(AimRight, ConsoleKey.RightArrow));

            //Spells
            InputHandler.AddListener(new KeyListener(ShootFireball, ConsoleKey.Spacebar));
            InputHandler.AddListener(new KeyListener(FreezeSpell, ConsoleKey.V));

        }

        public override void Update()
        {
            base.Update();
            if (fireballAim && fireballAimFlashCooldown.IsCooldownDone())
            {
                fireballAimFlashCooldown.StartCooldown();
                fireballAimSwitch = !fireballAimSwitch;
                renderer.color = fireballAimSwitch ? fireballAimFlashColor1 : fireballAimFlashColor2;
            }
            else if (!fireballAim)
            {
                renderer.color = baseColor;
            }
        }

        public bool Test()
        {
            if (testStrength > 20)
            {
                testStrength = 20;
            }
            else
            {
                testStrength = 100;
            }
            return true;
        }
        public bool OnCollision(BaseObject baseObject)
        {
            return true;
        }
        public bool OnDeath()
        {
            Console.Clear();
            MiniDC.gamePlaying = false;
            RetryScreen.Display();

            return true;
        }
        void HealthCheck()
        {
            if (health.health <= 0)
            {
                Console.Clear();
                Console.WriteLine("You DIED \nPress any key to exit...");
                Console.ReadKey(true);
                Environment.Exit(0);
            }
        }

        public bool AimUp()
        {
            aimingIndicator.SetPosition(position.x, position.y - 1);
            aimingIndicator.direction = Movement.Direction.North;
            return true;
        }
        public bool AimDown()
        {
            aimingIndicator.SetPosition(position.x, position.y + 1);
            aimingIndicator.direction = Movement.Direction.South;
            return true;
        }
        public bool AimLeft()
        {
            aimingIndicator.SetPosition(position.x - 1, position.y);
            aimingIndicator.direction = Movement.Direction.West;
            return true;
        }
        public bool AimRight()
        {
            aimingIndicator.SetPosition(position.x + 1, position.y);
            aimingIndicator.direction = Movement.Direction.East;
            return true;
        }

        public bool MoveUp()
        {
            if (movement.Move(Movement.Direction.North))
            {
                aimingIndicator.position.y -= 1;
            }
            direction = Movement.Direction.North;

            return true;
        }
        public bool MoveDown()
        {
            if (movement.Move(Movement.Direction.South))
            {
                aimingIndicator.position.y += 1;
            }
            direction = Movement.Direction.South;
            return true;
        }
        public bool MoveLeft()
        {
            if (movement.Move(Movement.Direction.West))
            {
                aimingIndicator.position.x -= 1;
            }
            direction = Movement.Direction.West;
            return true;
        }
        public bool MoveRight()
        {
            if (movement.Move(Movement.Direction.East))
            {
                aimingIndicator.position.x += 1;
            }
            direction = Movement.Direction.East;
            return true;
        }

        public bool FreezeSpell()
        {
            GetMap().newObjects.Add(new FreezeSource(GetMap(), (position.x, position.y), freezeStrength));
            return true;
        }

        public bool ShootFireball()
        {
            if (!GetMap().GetObjectsAtPosition(aimingIndicator.position.x, aimingIndicator.position.y).Exists(x => x.obj.GetType() == typeof(Wall)))
            {
                GetMap().addingObjecsFuncs.Add(SpawnFireball);
            }
            return true;
        }
        public bool SpawnIndicator()
        {
            aimingIndicator = new AimingIndicator(GetMap(), position.x, position.y, direction);
            GetMap().newObjects.Add(aimingIndicator);
            return true;
        }
        public bool SpawnFireball()
        {
            GetMap().newObjects.Add(new Fireball(GetMap(), aimingIndicator.position.x, aimingIndicator.position.y, aimingIndicator.direction, fireballStrength, fireballSpeed));
            return true;
        }



    }
}
