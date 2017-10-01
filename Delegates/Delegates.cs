...
    class Program
    {
        ...
            // "control system all machines"
            ControlSystem sys = new ControlSystem();

            // Machine_2
            Machine_2 machine_2 = new Machine_2();
            sys.Start = machine_2.startMachine2;                       // add *START* machine_2
            sys.ShutDown = machine_2.shutDownMachine2;                 // add *SHUTDOWN* machine_2
            // Machine_3
            Machine_3 machine_3 = new Machine_3();
            sys.Start = (() => { machine_3.startMachine3(0); });       // add *START* machine_3
            sys.ShutDown = (() => { machine_3.shutDownMachine3(0); }); // add *SHUTDOWN* machine_3

            // *START* all machines
            Console.Write("Press any key to *START* all machines: "); Console.ReadKey();
            sys.Start();
            // *SHUTDOWN* all machines
            Console.Write("\nPress any key to *SHUTDOWN* all machines: "); Console.ReadKey();
            sys.ShutDown();
            // exit
            Console.Write("\nPress any key for *EXIT* from the system: "); Console.ReadKey();
    }

    class ControlSystem // "control system all machines" this code never change
    {
        public ControlSystem() {
            this.start += startMachine1;
            this.shutDown += shutDownMachine1; }
        private void startMachine1()
        { Console.WriteLine("\nMachine_1 *START*"); }
        private void shutDownMachine1()
        { Console.WriteLine("\nMachine_1 *SHUTDOWN*"); }

        // delegate
        public delegate void Delegate();
        private Delegate start;
        private Delegate shutDown;

        // *START* all machines
        public Delegate Start
        { get { return this.start; } set { this.start += value; } }
        // *SHUTDOWN* all machines
        public Delegate ShutDown
        { get { return this.shutDown; } set { this.shutDown += value; } }
    }

    class Machine_2 {
        public void startMachine2()
        { Console.WriteLine("Machine_2 *START*"); }
        public void shutDownMachine2()
        { Console.WriteLine("Machine_2 *SHUTDOWN*"); } }

    class Machine_3 {
        public void startMachine3(int param)
        { Console.WriteLine("Machine_3 *START*"); }
        public void shutDownMachine3(int param)
        { Console.WriteLine("Machine_3 *SHUTDOWN*"); } }
