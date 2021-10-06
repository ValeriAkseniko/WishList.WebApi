﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WishList.Entities.Models
{
    public class Profile
    {
        public Guid Id { get; set; }

        public Guid AccountId { get; set; }
        public Account Account { get; set; }

        public string Nickname { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }

        public List<WishList> WishLists { get; set; }
    }
}
