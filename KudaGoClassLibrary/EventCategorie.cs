using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace KudaGoClassLibrary
{
    public class EventCategorie
    {
        public string Name { get; set; }
        public string Slug { get; set; }
    }

    public class EventsParsing
    {
        public int counts;
        public string next;
        public string previous;
        public List<Event> results;
    }

    public class Event
    {
        public string id;
        public string publication_date; // дата публикации
        public List<Dates> dates; // даты проведения
        public string title; // название
        public string short_title; // короткое название
        public string slug; // слаг
        public Place place; // место проведения
        public string description; // описание
        public string body_text; // полное описание
        public Location location; // город проведения
        public List<string> categories; // список категорий
        public string tagline; // тэглайн
        public string age_restriction; // возрастное ограничение
        public string price; // стоимость
        public bool is_free; // бесплатное ли событие
        public List<Image> images; // картинки
        public int favorites_count; // сколько пользователей добавило событие в избранное
        public int comments_count; // число комментариев к событию
        public string site_url; // адрес события на сайте kudago.com
        public List<string> tags; // тэги события
        public List<Participant> participants; // агенты события
    }

    public class Location
    {
        public string slug;
        public string name;
        public string timezone;
        public Coords coords;
        public string language;
        public string currency;  
    }

    public class Coords
    {
        public double lat;
        public double lon;
    }

    public class Place
    {
        public int Id;
        public string title;
        public string slug;
        public string address;
        public string phone;
        public bool is_stub;
        public string site_url;
        public Coords coords;
        public string subway;
        public bool is_closed;
        public string location;
    }
    public class Dates
    {
        public DateTime StartDate; 
        public DateTime EndDate;
        public string start;
        public string end;
        public bool is_continuous;
        public bool is_endless;
        public bool is_startless;
        public List<Schedule> schedules;
        public bool use_place_schedule;

        public string start_date
        {
            set
            {
                if(value != null)
                {
                    StartDate = new DateTime(Int32.Parse(value.Substring(0, 4)), Int32.Parse(value.Substring(5, 2)), Int32.Parse(value.Substring(8, 2)));
                }                
            }
        }
        public string end_date
        {
            set
            {
                if (value != null)
                {
                    EndDate = new DateTime(Int32.Parse(value.Substring(0, 4)), Int32.Parse(value.Substring(5, 2)), Int32.Parse(value.Substring(8, 2)));
                }
            }
        }
    }

    public class Schedule
    {
        public string start_time;
        public string end_time;
        public List<DayOfWeek> days_of_week;
    }

    public class Image
    {
        public string image;
        public Source source;
        public Thumbnails thumbnails;
        
        public class Source
        {
            public string link;
            public string name;
        }
        public class Thumbnails
        {           
            public string _144x96;
            public string _640x384;            
        }
    }
    public class Participant
    {
        public class Role
        {
            public int id;
            public string name;
            public string name_plural;
            public string slug;
        }
        public class Agent
        {
            public int id;
            public string ctype;
            public string title;
            public string slug;
            public string description;
            public string body_text;
            public string rank;
            public string agent_type;
            public List<Image> images;
            public int favorites_count;
            public int comments_count;
            public string item_url;
            public bool disable_comments;
        }
    }    
}
