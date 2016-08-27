using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Homeclick.Models
{
    public partial class Floor
    {
        vinabits_homeclickEntities db = new vinabits_homeclickEntities();

        public IList<Room> getRooms()
        {
            IList<Room> canvas = new List<Room>();
            canvas = (from room in db.Rooms
                     where room.Floor.Id == this.Id
                     select room).ToList<Room>();
            return canvas;
        }
    }
}