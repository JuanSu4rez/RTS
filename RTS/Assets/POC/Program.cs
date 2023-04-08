using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Remaking {
    class Program {
        static void FakeMain(string[] args) {
            //WHAT SHOULD BE A MONOBEHAVIOUR
            PhaseOne();
        }

        private static void PhaseOne() {
            //resource
            Resource bush = new Resource(100);
            bush.Position = new Vector3(1, 1, 1);

            var player = new Player(1000);

            // Building
            var building = new Building("Urban Center", cost: 100, health: 100);
            player.Buildings.Add(building);

            var gatheringSpeed = new GatheringSpeed(1);
            var gatheringCapacity = new Capacity(0, 100, gatheringSpeed);
            //worker
            var workerA = new Worker("workerA", 100, gatheringCapacity);
            player.Workers.Add(workerA);


            var gatherFromTheBush = new TaskGatheringManager(
                resource: bush,
                pointToDeposit: new Vector3(),
                workerA,
                building,
                // use events
                player.AddFood
                );
            gatherFromTheBush.Init();
            gatherFromTheBush.Execute();
            gatherFromTheBush.Execute();
            gatherFromTheBush.Execute();
            gatherFromTheBush.Execute();

            Console.WriteLine(player.FoodAmount);
            Console.ReadLine();

        }
    }

    public class Player {
        private float _foodAmount;
        public List<Unit> Workers { get; set; }
        public List<Building> Buildings { get; set; }
        public float FoodAmount { get => _foodAmount; set => _foodAmount = value; }

        public Player(float foodamount) {
            FoodAmount = foodamount;
            Workers = new List<Unit>();
            Buildings = new List<Building>();
        }

        public void AddFood(float collectedAmount) {
            FoodAmount = _foodAmount+collectedAmount;
        }
    }

    public class Vector3 {
        public float X;
        public float Y;
        public float Z;

        public Vector3() {

        }
        public Vector3(float x, float y, float z) {
            X = x;
            Y = y;
            Z = z;

        }
    }
    //KIND OF GAME OBJECT
    public class Building {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Cost { get; set; }
        public Building(string name, int cost, int health) {
            Name = name;
            Cost = cost;
            Health = health;
        }
    }

    //KIND OF GAME OBJECT
    public class Unit {
        public string Name { get; set; }
        public int Health { get; set; }
        public Unit(string name, int health) {
            Name = name;
            Health = health;
        }
    }

    public class GatheringSpeed {
        public int Value { get; set; }
        public GatheringSpeed(int value) {
            Value = value;
        }
    }

    public class Capacity {
        public int Current { get; set; }
        public int Limit { get; set; }
        public GatheringSpeed GatheringSpeed { get; set; }

        public Capacity(int current, int limit, GatheringSpeed gatheringSpeed) {
            Current = current;
            Limit = limit;
            GatheringSpeed = gatheringSpeed;
        }


    }

    //KIND OF GAME OBJECT
    public class Worker : Unit {
        public Capacity GatheringCapacity { get; set; }
        public Worker(string name, int health, Capacity gatheringCapacity) : base(name, health) {
            GatheringCapacity = gatheringCapacity;
        }
    }
    //KIND OF GAME OBJECT
    public class Resource {
        public float Amount { get; set; }
        public Vector3 Position { get; internal set; }

        public Resource(float amount) {
            Amount = amount;
        }

        public int DiscountAmount(int discount) {
            int discountedAmount = (int)( Amount - discount );
            Amount = ( discountedAmount < 0 ) ? 0 : discountedAmount;
            return discount;
        }
    }

    //SHOULD BE A Task
    public abstract class Task {

        public abstract void Execute();

        public abstract bool IsFinished();
    }

    public class GoTask : Task {
        public Vector3 Destination { get; set; }
        public Unit Unit { get; set; }


        public GoTask(Unit unit, Vector3 destination) {
            Unit = unit;
            Destination = destination;
        }

        public override void Execute() {
            //Move till you are in position
        }

        public override bool IsFinished() {
            // RESTA
            return true;
        }
    }

    public class GatherResourceTask : Task {

        public Worker Worker { get; set; }
        public Resource Resource { get; set; }

        public GatherResourceTask(Worker worker, Resource resource) {
            Worker = worker;
            Resource = resource;
        }

        public override void Execute() {
            // Execute the gather resource task logic here
            var discounted = Resource.DiscountAmount(Worker.GatheringCapacity.GatheringSpeed.Value);
            Worker.GatheringCapacity.Current += discounted;
        }

        public override bool IsFinished() {
            return true;// Resource.Amount <= 0;
        }
    }

    public class DepositResourceTask : Task {
        private readonly Worker _worker;
        private Action<float> _collectAmount;
        public DepositResourceTask(Worker worker, Action<float> collectAmount) {
            _worker = worker;
            _collectAmount = collectAmount;
        }

        public override bool IsFinished() {
            //
            return true;
        }

        public override void Execute() {
            //
            var amount = _worker.GatheringCapacity.Current;
            _collectAmount(amount);
            _worker.GatheringCapacity.Current = 0;
        }
    }

    public class TaskExecutor {
        private Queue<Task> _taskQueue;
        private Task currentTask = null;
        public TaskExecutor() {
            _taskQueue = new Queue<Task>();
        }

        public void QueueTask(Task task) {
            _taskQueue.Enqueue(task);
        }

       

        public void Execute() {

            if(_taskQueue.Count > 0) {

                if(currentTask == null) {
                    currentTask = _taskQueue.Peek();
                }

                currentTask.Execute();
                if(currentTask.IsFinished()) {
                    _taskQueue.Dequeue();
                    currentTask = _taskQueue.Count > 0 ? _taskQueue.Peek() : null;
                }

            }
        }
    }

    public class TaskGatheringManager {
        private Resource resource;
        private Vector3 pointToDeposit;
        private TaskExecutor taskExecutor;
        private Building building;
        private Worker worker;
        private Action<float> collectAmount;
        public TaskGatheringManager(Resource resource, Vector3 pointToDeposit, Worker worker, Building building, Action<float> collectAmount) {
            this.resource = resource;
            this.pointToDeposit = pointToDeposit;
            this.worker = worker;
            this.building = building;
            this.collectAmount = collectAmount;
            this.taskExecutor = new TaskExecutor();
        }

        public void Init() {

            taskExecutor.QueueTask(new GoTask(worker, resource.Position));
            taskExecutor.QueueTask(new GatherResourceTask(worker, resource));
            taskExecutor.QueueTask(new GoTask(worker, pointToDeposit));
            taskExecutor.QueueTask(new DepositResourceTask(worker, this.collectAmount));
        }

        public void Execute() {
            if(resource.Amount > 0) {
                taskExecutor.Execute();
            }
            else {
                Init();
            }
        }
    }
}
