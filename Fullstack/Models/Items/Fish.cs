using Fullstack.Models.Static;

namespace Fullstack.Models.Items
{
    public class Fish : Item
    {
        public string? Name { get; set; }
        public decimal Size { get; set; }
        public string? Description { get; set; }

        public Fish()
        {
            Name = GetType().Name;
        }
    }

    public class Pike : Fish
    {
        public Pike() : base()
        {
            Size = Helpers.GetRandomDecimal(0.2, 1.6);
            Description = "Pike fish, known for their ambush hunting style and elongated bodies, are voracious predators found in freshwater habitats.";
        }
    }

    public class Salmon : Fish
    {
        public Salmon() : base()
        {
            Size = Helpers.GetRandomDecimal(0.5, 1.6);
            Description = "Salmon fish, known for their migratory behavior and pink flesh, are found in both freshwater and saltwater habitats.";
        }
    }

    public class Trout : Fish
    {
        public Trout() : base()
        {
            Size = Helpers.GetRandomDecimal(0.1, 1.0);
            Description = "Trout fish, known for their colorful patterns and streamlined bodies, are found in freshwater habitats.";
        }
    }

    public class Catfish : Fish
    {
        public Catfish() : base()
        {
            Size = Helpers.GetRandomDecimal(0.3, 1.5);
            Description = "Catfish, known for their barbels and whisker-like sensory organs, are found in freshwater habitats.";
        }
    }

    public class Burbot : Fish
    {
        public Burbot() : base()
        {
            Size = Helpers.GetRandomDecimal(0.2, 1.0);
            Description = "Burbot fish, known for their elongated bodies and mottled appearance, are found in freshwater habitats.";
        }
    }

    public class YellowPerch : Fish
    {
        public YellowPerch() : base()
        {
            Size = Helpers.GetRandomDecimal(0.1, 0.5);
            Description = "Yellow perch, known for their yellow coloration and spiny dorsal fins, are found in freshwater habitats.";
        }
    }

    public class Whitefish : Fish
    {
        public Whitefish() : base()
        {
            Size = Helpers.GetRandomDecimal(0.3, 1.0);
            Description = "Whitefish, known for their silvery coloration and forked tails, are found in freshwater habitats.";
        }

    }

    public class Walleye : Fish
    {
        public Walleye() : base()
        {
            Size = Helpers.GetRandomDecimal(0.3, 1.0);
            Description = "Walleye fish, known for their large eyes and sharp teeth, are found in freshwater habitats.";
        }
    }

    public class Sturgeon : Fish
    {
        public Sturgeon() : base()
        {
            Size = Helpers.GetRandomDecimal(0.5, 2.0);
            Description = "Sturgeon fish, known for their elongated bodies and bony plates, are found in freshwater habitats.";
        }
    }

    public class Bass : Fish
    {
        public Bass() : base()
        {
            Size = Helpers.GetRandomDecimal(0.3, 1.0);
            Description = "Bass fish, known for their aggressive behavior and striped bodies, are found in freshwater habitats.";
        }
    }


}
