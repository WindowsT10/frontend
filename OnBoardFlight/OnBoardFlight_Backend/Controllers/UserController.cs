﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnBoardFlight_Backend.Data.DTO;
using OnBoardFlight_Backend.Data.IRepository;
using OnBoardFlight_Backend.Model;

namespace OnBoardFlight_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("passengers")]
        public IEnumerable<User> GetPassengers()
        {
            return _userRepository.GetPassengers().OrderBy(p => (p as Passenger).Seat);
        }

        [HttpPost("cabincrew/login")]
        public IActionResult GetCabinCrewMemberByCredentials(CabinCrewLogin cabinCrewLogin)
        {
            User cr = _userRepository.GetCabinCrewByCredentials(cabinCrewLogin.Login, cabinCrewLogin.Password);
            if (cr == null)
            {
                return BadRequest();
            }
            return Ok(cr);
        }

        [HttpGet("passenger/{seat}")]
        public IActionResult GetPassengerBySeat(string seat)
        {
            Passenger passenger = _userRepository.GetPassengerBySeat(seat);
            if (passenger == null)
            {
                return NotFound();
            }
            return Ok(new PassengerDTO(passenger));
        }

        [HttpPut]
        [Route("passenger/update")]
        public IActionResult UpdatePassenger(ChangeSeatsPassengersDTO passengerDTO)
        {
            if (passengerDTO == null)
            {
                return BadRequest();
            }
            Passenger passenger1 = _userRepository.GetPassengerById(passengerDTO.UserId1);
            Passenger passenger2 = _userRepository.GetPassengerById(passengerDTO.UserId2);
            _userRepository.ChangeSeats(passenger1, passenger2);
            _userRepository.SaveChanges();
            return NoContent();
        }

        [HttpGet("notifications/{id}")]
        public IEnumerable<Notification> GetPassengerNotificationsById(int id)
        {
            return _userRepository.GetNotificationByPassengerId(id);
        }

        [HttpPost]
        [Route("notification/add")]
        public IActionResult AddNotification(Notification notification)
        {
            if(notification == null)
            {
                return BadRequest();
            }
            if (notification.General)
            {
                foreach(Passenger passenger in _userRepository.GetPassengers())
                {
                    Notification not = new Notification
                    {
                        Content = notification.Content,
                        General = notification.General,
                        PassengerSeat = notification.PassengerSeat,
                        Title = notification.Title
                    };
                    passenger.AddNotification(not);
                    _userRepository.UpdatePassenger(passenger);
                    _userRepository.SaveChanges();
                }
            }
            else
            {
                Passenger passenger = _userRepository.GetPassengerBySeat(notification.PassengerSeat);
                passenger.AddNotification(notification);
                _userRepository.UpdatePassenger(passenger);
                _userRepository.SaveChanges();
            }

            return Ok();
        }
    }
}