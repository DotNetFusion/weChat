using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase;
using Supabase.Postgrest.Models;
using Supabase.Realtime;

namespace weChat.services
{ 
    public static class SupabaseService
    {
        private static Supabase.Client _client;

        public static async Task Init()
        {
            var url = "https://olmcokegdcnkuwugcjky.supabase.co";
            var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im9sbWNva2VnZGNua3V3dWdjamt5Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDkzMDYwNTgsImV4cCI6MjA2NDg4MjA1OH0.Xl-TDvzuJ9zyV5ca4oYrXVhV6Gl4v7UlNyEh9CA3otU";

            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };

            _client = new Supabase.Client(url, key, options);
            await _client.InitializeAsync();
        }

        public static Supabase.Client Client => _client;
    }

}
