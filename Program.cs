using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport_Management_Program
{
    public enum FlightStatus
    {
        takeOffApproved,
        takeoffToBeRequested,
        ascended
    };  
    class Track
    {
        private string Code;
        public string code
        {
            get
            {
                return this.Code;
            }
            set
            {
                this.Code = value;
            }
        }
        private bool Free;
        public bool free
        {
            set
            {
                this.Free = value;
            }
            get
            {
                return this.Free;
            }
        }
        public Track()
        {
            this.code = "0000";
            this.free = false;
        }
        public Track(string c)
        {
            this.code = c;
            this.free = true;
        }
        public string GiveDescription()
        {
            string isFree;
            if (free)
            {
                isFree = "Yes";
            }
            else
            {
                isFree = "No";
            }
            return "Code: " + this.code + " / Free:"+isFree;
        }

    }
    class AirCraft
    {
        private string flightNumber;
        public string flight_number {
            get {
                return flightNumber;
            }
            set {
                flightNumber = value;
            }
        }
        private Track assignedTrack;
        public Track assigned_track
        {
            get
            {
                return assignedTrack;
            }
            set
            {
                this.assignedTrack = value;
            }
        }
        FlightStatus Status;
        public FlightStatus status
        {
            set
            {
                this.Status = value;
            }
            get
            {
                return this.Status;
            }
        }
        public string TakeOff()
        {
            string ret = "";
            if(Status == FlightStatus.takeOffApproved)
            {
                Status = FlightStatus.ascended;
                assigned_track.free = true;
                ret = "Flight with flight number " + flight_number + " took off on track " + assigned_track.code;
                assigned_track = null;
            }
            else
            {
                ret = "Takeoff not possible – Current status: " + this.status;
            }
            return ret;
        }
        public string GiveDescription()
        {
            return "Flight Number: " + this.flight_number + "/ Status: " + this.status + " / Assigned Track: " + this.assigned_track;
        }
    }
    class ControlTower
    {
        List<AirCraft> Aeroplanes = new List<AirCraft>();
        List<Track> Tracks = new List<Track>();
        public string RegisterAircraft(AirCraft craft)
        {
            string flightnumber = string.Empty;
            Random random = new Random();
            do
            {
                int asciiNr = random.Next(48, 91);
                if (asciiNr >= 58 && asciiNr < 65)
                {
                    continue;
                }
                flightnumber += (char)asciiNr;
            } while (flightnumber.Length < 6);
            craft.flight_number = flightnumber;
            craft.status = FlightStatus.takeoffToBeRequested;
            Aeroplanes.Add(craft);
            return flightnumber;
        }
        
        public void AddTrack(string code)
        {
            Track track = new Track(code);
            Tracks.Add(track);
        }
        public AirCraft SearchAircraft(string flightNumber)
        {
            foreach (AirCraft plane in Aeroplanes)
            {
                if(plane.flight_number == flightNumber)
                {
                    return plane;
                }
            }
            return null;
        }
        private Track SearchFreeLane()
        {
            foreach(Track track in Tracks)
            {
                if (track.free)
                    return track;
            }
            return null;
        }
        public string RequestTakeOff(string flightNumber)
        {
            string ret = "";
            AirCraft current = SearchAircraft(flightNumber);
            Track freeLane = SearchFreeLane();
            if (current != null)
            {
                if (current.status == FlightStatus.takeoffToBeRequested)
                {
                    if (freeLane != null)
                    {
                        current.status = FlightStatus.takeOffApproved;
                        current.assigned_track = freeLane;
                        ret = "Take Off Request approved - Assigned to track " + freeLane.code;
                    }
                    else
                        ret = "Take Off not approved - No free track";
                }
                else
                    ret = "Take Off not approved - Aeroplane Status not matching - TakeOffRequest denied";
            }
            else
                ret = "Take Off not approved - Aeroplane not found";
            return ret;
        }
        public string OverviewAircraft()
        {
            string overView = "";
            foreach (AirCraft plane in Aeroplanes)
            {
                overView += ("  -  "+plane.GiveDescription() + '\n');
            }
            return overView;
        }
        public string OverviewTracks()
        {
            string overView = "";
            foreach(Track track in Tracks)
            {
                overView += ("  -  " + track.GiveDescription() + '\n');
            }
            return overView;
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            ControlTower controlTower = new ControlTower();
            int choice = 0;
            while (choice != 6)
            {
                Console.WriteLine("============================================");
                Console.WriteLine("              CONTROL TOWER MENU");
                Console.WriteLine("============================================");
                Console.WriteLine("\t1. Add track");
                Console.WriteLine("\t2. Register Aeroplane");
                Console.WriteLine("\t3. Control Tower Overview");
                Console.WriteLine("\t4. Request Take Off");
                Console.WriteLine("\t5. Take Off");
                Console.WriteLine("\t6. Stop");
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Enter choice : ");
                choice = Convert.ToInt32(Console.ReadLine());
                if (choice > 6 || choice < 1)
                {
                    Console.WriteLine("Choose from 1-6 only ! ");
                }
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter Track Code : ");
                        string tc = Console.ReadLine();
                        controlTower.AddTrack(tc);
                        Console.WriteLine("\nTrack OverView : ");
                        Console.WriteLine(controlTower.OverviewTracks());
                        break;
                    case 2:
                        string fNo = controlTower.RegisterAircraft(new AirCraft());
                        Console.WriteLine("Aeroplane was registered. Flight Number : " + fNo);
                        Console.WriteLine("\nAircrafts OverView : ");
                        Console.WriteLine(controlTower.OverviewAircraft());
                        break;
                    case 3:
                        Console.WriteLine("Track OverView : ");
                        Console.WriteLine( controlTower.OverviewTracks());
                        Console.WriteLine("Aircrafts OverView : ");
                        Console.WriteLine(controlTower.OverviewAircraft());
                        break;
                    case 4:
                        Console.WriteLine("Enter the flight number : ");
                        fNo = Console.ReadLine();
                        Console.WriteLine(controlTower.RequestTakeOff(fNo));
                        break;
                    case 5:
                        Console.WriteLine("Enter the flight number : ");
                        fNo = Console.ReadLine();
                        AirCraft craft = controlTower.SearchAircraft(fNo);
                        Console.WriteLine(craft.TakeOff());
                        break;
                    case 6:
                        break;
                }
            }
        }
    }
} 
