﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catan.Shared.Model
{
	public class Actor
	{
		public string Name { get; set; }
		public string Token { get; set; }
        public Actor(string name, string token)
        {
            Name = name;
            Token = token;
        }
    }
}
