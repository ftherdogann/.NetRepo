﻿using System.Collections.Generic;

namespace SignalR.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Team Team { get; set; }
    }
}
