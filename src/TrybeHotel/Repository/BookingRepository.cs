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
                Room = room
            };
            _context.Bookings.Add(bookingToSave);
            _context.SaveChanges();
            return new BookingResponse
            {
                BookingId = bookingToSave.BookingId,
                CheckIn = bookingToSave.CheckIn,
                CheckOut = bookingToSave.CheckOut,
                GuestQuant = bookingToSave.GuestQuant,
                Room = room,
            };

        }

        public BookingResponse GetBooking(int bookingId, string email)
        {
            var booking = _context.Bookings.Include(b => b.Room).ThenInclude(r => r!.Hotel).FirstOrDefault(b => b.BookingId == bookingId && b.User!.Email == email);
            if (booking == null)
            {
                return null!;
            }
            return new BookingResponse
            {
                BookingId = booking.BookingId,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut,
                GuestQuant = booking.GuestQuant,
                Room = booking.Room,
            };
        }

        public Room GetRoomById(int RoomId)
        {
            var room = _context.Rooms.Include(r => r.Hotel).FirstOrDefault(r => r.RoomId == RoomId);

            return room!;
        }

    }

}