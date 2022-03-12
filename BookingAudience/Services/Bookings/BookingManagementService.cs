using BookingAudience.DAL.Repositories;
using BookingAudience.Models;
using BookingAudience.Models.Users;
using BookingAudience.Services.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingAudience.Services.Bookings
{
    public class BookingManagementService
    {
        private readonly IGenericRepository<AppUser> usersRepository;
        private readonly UserAuthService userAuthService;
        private readonly UserManager<AppUser> userManager;
        private readonly IGenericRepository<BookingAudience.Models.Booking> bookingRepository;

        private readonly DateTime startPossibleTime;
        private readonly DateTime endPossibleTime;

        public int CurrentUserId
        {
            get
            {
                return userAuthService.CurrentUserId;
            }
        }
        public BookingManagementService(
            IGenericRepository<AppUser> usersRepository,
            IGenericRepository<BookingAudience.Models.Booking> bookingRepository,
            UserAuthService userAuthService,
            UserManager<AppUser> userManager)
        {
            this.usersRepository = usersRepository;
            this.bookingRepository = bookingRepository;
            this.userAuthService = userAuthService;
            this.userManager = userManager;
        }

        public async Task BookAudienceAsync(IGenericRepository<BookingAudience.Models.Booking> bookingRepository, BookingAudience.Models.Booking booking)
        {
            if (booking.BookingTime >= DateTime.Now)
                throw new Exception("Уже поздно бронировать на это время");
            if (booking.BookingTime.Minute % 30 != 0)
                throw new Exception("Продолжительность аренды должна быть кратна 30 минутам");
            if (booking.Creator.Id != userAuthService.CurrentUserId)
                throw new Exception("Бронировать аудитории можно только на своё имя");
            if (booking.DurationInMinutes <= 0)
                throw new Exception("Продолжительность бронирования должна быть больше 0");

            //todo проверять что эта аудиенция на это время никем не занята ещё

            await bookingRepository.CreateAsync(booking);
        }

        public async Task UnbookAudience(IGenericRepository<BookingAudience.Models.Booking> bookingRepository, BookingAudience.Models.Booking booking)
        {
            if (booking.BookingTime >= DateTime.Now)
                throw new Exception("Уже поздно отказаться от брони");
            await bookingRepository.DeleteAsync(booking.Id);
        }

        public async Task<List<BookingAudience.Models.Booking>> GetCurrentUsersLastBooks()
        {
            AppUser currentUser = await usersRepository.GetAsync(userAuthService.CurrentUserId);
            return bookingRepository.Get().Where(b => DateTime.Now >= b.BookingTime && b.Creator.Id == currentUser.Id).ToList();
        }

        public async Task<List<BookingAudience.Models.Booking>> GetCurrentUsersFutureBooks()
        {
            AppUser currentUser = await usersRepository.GetAsync(userAuthService.CurrentUserId);
            return bookingRepository.Get().Where(b => DateTime.Now < b.BookingTime && b.Creator.Id == currentUser.Id).ToList();
        }

        /// <summary>
        /// получить словарь доступного времени в этот день на эту аудиторию
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<Dictionary<DateTime, bool>> GetTimeScale(Audience audience, DateTime date)
        {
            List<Booking> bookings = bookingRepository.Get().Where(b => b.BookedAudience.Id == audience.Id).ToList();

            DateTime time = DateTime.Now;
            if (time.Minute > 30)
            {
                time = time.AddMinutes(-time.Minute);
            }
            else
            {
                time = time.AddMinutes(Math.Abs(30 - time.Minute));
            }

            //проверка что начальное время в диапазоне
            if (time.TimeOfDay.TotalSeconds > startPossibleTime.TimeOfDay.TotalSeconds ||
                time.TimeOfDay.TotalSeconds < endPossibleTime.TimeOfDay.TotalSeconds)
            {
                throw new Exception("На это время нельзя арендовать помещение");
            }

            //todo проверить в рандже ли запрашиваемый диапазон
            //todo щас добавить time в result
            Dictionary<DateTime, bool> result = new Dictionary<DateTime, bool>();
            for (int i = 0; ; i++)
            {
                //todo тут докинуть 30 минут и проверить в ранже ли ещё
                //result.Add()
            }
        }
    }
}
