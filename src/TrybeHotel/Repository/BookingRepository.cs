using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.EntityFrameworkCore;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
            var room = GetRoomById(booking.RoomId);
            var user = _context.Users.Where(u => u.Email == email).FirstOrDefault();
            if (room == null || user == null || booking.GuestQuant > room.Capacity)
            {
                return null!;
            }
            var bookingToSave = new Booking
            {
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                RoomId = booking.RoomId,
                UserId = user.UserId,
            };
            _context.Bookings.Add(bookingToSave);
            _context.SaveChanges();
            var city = _context.Cities.Find(room.Hotel!.CityId);
            return new BookingResponse
            {
                BookingId = bookingToSave.BookingId,
                CheckIn = bookingToSave.CheckIn,
                CheckOut = bookingToSave.CheckOut,
                GuestQuant = bookingToSave.GuestQuant,
                Room = new RoomDto {
                    RoomId = room.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    HotelId = room.HotelId,
                    Hotel = new HotelDto {
                        HotelId = room.Hotel!.HotelId,
                        Name = room.Hotel.Name,
                        Address = room.Hotel.Address,
                        CityId = room.Hotel.CityId,
                        CityName = city != null ? city.Name : "",
                        State = city != null ? city.State : "",
                    }
                },
            };

        }

        public BookingResponse GetBooking(int bookingId, string email)
        {
            var booking = _context.Bookings.Include(b => b.Room).ThenInclude(r => r!.Hotel).FirstOrDefault(b => b.BookingId == bookingId && b.User!.Email == email);
            if (booking == null)
            {
                return null!;
            }
            var city = _context.Cities.Find(booking.Room!.Hotel!.CityId);
            return new BookingResponse
            {
                BookingId = booking.BookingId,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                Room = new RoomDto {
                    RoomId = booking.Room.RoomId,
                    Name = booking.Room.Name,
                    Capacity = booking.Room.Capacity,
                    Image = booking.Room.Image,
                    HotelId = booking.Room.HotelId,
                    Hotel = new HotelDto {
                        HotelId = booking.Room.Hotel!.HotelId,
                        Name = booking.Room.Hotel.Name,
                        Address = booking.Room.Hotel.Address,
                        CityId = booking.Room.Hotel.CityId,
                        CityName = city != null ? city.Name : "",
                        State = city != null ? city.State : "",
                    }
                },
            };
        }

        public Room GetRoomById(int RoomId)
        {
            var room = _context.Rooms.Include(r => r.Hotel).FirstOrDefault(r => r.RoomId == RoomId);

            return room!;
        }

    }

}