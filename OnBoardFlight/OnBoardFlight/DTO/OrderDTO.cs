﻿using OnBoardFlight.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnBoardFlight.DTO
{
    public class OrderDTO
    {

        public string SeatNumber { get; set; }

        public DateTime Time { get; set; }

        public IEnumerable<OrderlineDTO> OrderlineDTOs { get; set; }


        public OrderDTO(Order order)
        {
            ICollection<OrderlineDTO> olDTOs = new List<OrderlineDTO>();
            SeatNumber = order.SeatNumber;
            Time = order.Time;
            foreach(Orderline ol in order.Orderlines)
            {
                OrderlineDTO olDTO = new OrderlineDTO(ol);
                olDTOs.Add(olDTO);
            }
            OrderlineDTOs = olDTOs;
        }
    }
}