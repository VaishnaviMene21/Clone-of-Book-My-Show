﻿namespace book_my_show.Model
{
    public class User
    {

        public int Id { get; set; }
        public string? Username { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;

        public string? Location { get; set; } = string.Empty;

    }
}
