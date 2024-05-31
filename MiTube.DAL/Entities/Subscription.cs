using System;
using System.ComponentModel.DataAnnotations;

namespace MiTube.DAL.Entities
{
    public class Subscription : Model
    {
        [Required]
        public Guid PublisherId { get; set; }
        public Guid SubscriberId { get; set; }

        //navigation properties
        virtual public User Publisher { get; set; }
        virtual public User Subscriber { get; set; }
        //virtual public IEnumerable<User> Publishers { get; set; }
        //virtual public IEnumerable<User> Subscribers { get; set; }
    }
}