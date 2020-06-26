using System;

namespace Codenation.Models
{
    public class Team
    {
        public Team(long id, string name, DateTime createDate, string mainShirtColor, string secondaryShirtColor)
        {
            Id = id;
            Name = name;
            CreateDate = createDate;
            MainShirtColor = mainShirtColor;
            SecondaryShirtColor = secondaryShirtColor;
        }

        public long Id { get; }
        public string Name { get; set; }

        public DateTime CreateDate { get; set; }

        public string MainShirtColor { get; set; } 
        public string SecondaryShirtColor { get; set; }
        public long IdCaptain { get; set; }

    }
}