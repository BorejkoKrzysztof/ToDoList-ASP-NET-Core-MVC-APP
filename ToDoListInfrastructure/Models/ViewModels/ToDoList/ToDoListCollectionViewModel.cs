﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoListInfrastructure.DTOs;

namespace ToDoListInfrastructure.Models.ViewModels.ToDoList
{
    public class ToDoListCollectionViewModel
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public IEnumerable<ToDoListDto> ToDoListCollection { get; set; }
        public PagingInfo PagingInfo { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
