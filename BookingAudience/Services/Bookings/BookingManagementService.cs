using BookingAudience.DAL.Repositories;
using BookingAudience.Extensions;
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
        private const int minimalStepInMinutes = 30;

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

            DateTime now = DateTime.Now;
            startPossibleTime = new DateTime(hour: 9, minute: 0, second: 0, year: now.Year, month: now.Month, day: now.Day);
            endPossibleTime = new DateTime(hour: 19, minute: 0, second: 0, year: now.Year, month: now.Month, day: now.Day);
        }

        public async Task BookAudienceAsync(IGenericRepository<BookingAudience.Models.Booking> bookingRepository, BookingAudience.Models.Booking booking)
        {
            if (booking.BookingTime >= DateTime.Now)
                throw new Exception("Уже поздно бронировать на это время");
            if (booking.DurationInMinutes < minimalStepInMinutes)
                throw new Exception($"Продолжительность бронирования должна быть больше {minimalStepInMinutes} минут");
            if (booking.BookingTime.Minute % 30 != 0)
                throw new Exception($"Продолжительность аренды должна быть кратна {minimalStepInMinutes} минутам");
            if (booking.Creator.Id != userAuthService.CurrentUserId)
                throw new Exception("Бронировать аудитории можно только на своё имя");


            //todo проверять что эта аудиенция на это время никем не занята ещё

            await bookingRepository.CreateAsync(booking);
        }

        public async Task UnbookAudience(IGenericRepository<Booking> bookingRepository, Booking booking)
        {
            if (booking.BookingTime >= DateTime.Now)
                throw new Exception("Уже поздно отказаться от бронирования");
            await bookingRepository.DeleteAsync(booking.Id);
        }

        public async Task<List<Booking>> GetCurrentUsersLastBooks()
        {
            AppUser currentUser = await usersRepository.GetAsync(userAuthService.CurrentUserId);
            return bookingRepository.Get().Where(b => DateTime.Now >= b.BookingTime && b.Creator.Id == currentUser.Id).ToList();
        }

        public async Task<List<Booking>> GetCurrentUsersFutureBooks()
        {
            AppUser currentUser = await usersRepository.GetAsync(userAuthService.CurrentUserId);
            return bookingRepository.Get().Where(b => DateTime.Now < b.BookingTime && b.Creator.Id == currentUser.Id).ToList();
        }

        /// <summary>
        /// получить словарь доступного времени в этот день на эту аудиторию
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public Dictionary<DateTime, bool> GetTimeScale(Audience audience, DateTime date)
        {
            List<Booking> bookings = bookingRepository.Get().Where(b => b.BookedAudience.Id == audience.Id).ToList();

            DateTime timeNow = DateTime.Now;
            date = new DateTime(hour: timeNow.Hour, minute: timeNow.Minute, second: timeNow.Second, 
                year: date.Year, month: date.Month, day: date.Day);
            //если берём на сегодняшнее число
            if (date.Date == timeNow.Date)
            {
                //если сейчас время меньше чем нижний минимальный порог, то устанавливаем время на нижний порог этот
                if (timeNow.TimeOfDay < startPossibleTime.TimeOfDay)
                {
                    date = new DateTime(hour: startPossibleTime.Hour, minute: startPossibleTime.Minute, second: startPossibleTime.Second, 
                        year: date.Year, month: date.Month, day: date.Day);
                }
                //иначе если время входит до верхнего порога, подгоняем до ближайшего
                else if (timeNow.TimeOfDay < endPossibleTime.TimeOfDay)
                {
                    if (timeNow.Minute > 30)
                    {
                        //округляем время до будущих 00 минут
                        timeNow = timeNow.AddMinutes(-timeNow.Minute).AddHours(1);
                    }
                    else
                    {
                        timeNow = timeNow.AddMinutes(Math.Abs(30 - timeNow.Minute));
                    }
                    date = new DateTime(hour: timeNow.Hour, minute: timeNow.Minute, second: timeNow.Second,
                        year: date.Year, month: date.Month, day: date.Day);
                }
                else
                {
                    throw new Exception("На это время нельзя арендовать помещение");
                }
            }
            else
            {
                timeNow = startPossibleTime;
                date = new DateTime(hour: timeNow.Hour, minute: timeNow.Minute, second: timeNow.Second,
                       year: date.Year, month: date.Month, day: date.Day);
            }

            Dictionary<DateTime, bool> result = new Dictionary<DateTime, bool>();

            while (true)
            {
                bool canBookForThisTime = true;

                if (date.TimeOfDay < endPossibleTime.TimeOfDay)
                {
                    //если текущая дата которую заносим в словарь имеет пересечения с любыми буками на эту комнату, то это время нельзя установить стартовым
                    if (bookings.Any(b => date.IsInRange(b.BookingTime, b.BookingTime.AddMinutes(b.DurationInMinutes))))
                    {
                        canBookForThisTime = false;
                    }
                    result.Add(date, canBookForThisTime);
                }
                else
                {
                    break;
                }

                //смещаем дату на следующую точку
                date = date.AddMinutes(minimalStepInMinutes);
            }

            return result;
        }
    }
}
