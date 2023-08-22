using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
            return _context.Rooms.Where(r => r.HotelId == HotelId).Select(r => new RoomDto
            {
                RoomId = r.RoomId,
                Name = r.Name,
                Capacity = r.Capacity,
                Image = r.Image,
                Hotel = new HotelDto
                {
                    HotelId = r.Hotel!.HotelId,
                    Name = r.Hotel.Name,
                    Address = r.Hotel.Address,
                    CityId = r.Hotel.CityId,
                    CityName = r.Hotel.City!.Name,
                    State = r.Hotel.City!.State,

                }
            }).ToList();
        }

        public RoomDto AddRoom(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
            var rooms = _context.Rooms;
            var savedRoom = from r in rooms
                            where r.RoomId == room.RoomId
                            select new RoomDto
                            {
                                RoomId = r.RoomId,
                                Name = r.Name,
                                Capacity = r.Capacity,
                                Image = r.Image,
                                Hotel = new HotelDto
                                {
                                    HotelId = r.Hotel!.HotelId,
                                    Name = r.Hotel.Name,
                                    Address = r.Hotel.Address,
                                    CityId = r.Hotel.CityId,
                                    CityName = r.Hotel.City!.Name,
                                    State = r.Hotel.City!.State,

                                }
                            };
            return savedRoom.First();
        }

        public void DeleteRoom(int RoomId)
        {
            var foundRoom = _context.Rooms.Find(RoomId);
            if (foundRoom != null)
            {
                _context.Rooms.Remove(foundRoom);
                _context.SaveChanges();
            }
        }
    }
}