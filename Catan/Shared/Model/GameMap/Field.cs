﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Catan.Shared.Model.GameMap
{
    public class Field
    {
        public int Id { get; set; }
        public int Number { get; set; }
        [JsonIgnore]
        public int HiddenNumber { get; set; }
        public TerrainType Type { get; set; }
        [JsonIgnore]
        public TerrainType HiddenType { get; set; }
        public bool IsRobbed { get; set; }
		[JsonIgnore]
		public Field[] Neighbours { get; set; } = new Field[6];
        public Edge[] Edges { get; set; } = new Edge[6];
        public Corner[] Corners { get; set; } = new Corner[6];

        public void RevealField()
        {
            Number = HiddenNumber;
            Type = HiddenType;
        }
        public void HideField()
        {
            HiddenNumber = Number;
            Number = 0;
            HiddenType = Type;
            Type = TerrainType.Unknown;
        }

        public override string ToString()
        {
            return "Szám: " + Number.ToString() + " Típus: " + Type.ToString();
        }
    }
}
