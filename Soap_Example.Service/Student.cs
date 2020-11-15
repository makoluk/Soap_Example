﻿using System;

namespace Soap_Example.Service
{
    public class Student
    {
        public int StudentID { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDay { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public bool Gender { get; set; }
    }
}