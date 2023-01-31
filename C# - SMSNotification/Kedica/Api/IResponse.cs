using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SMSNofication.Api
{
    public interface IResponse
    {
        string Status { get; set; }

        /// <summary>
        /// Indicates if the response can update
        /// </summary>
        bool CanUpdate { get; }

        /// <summary>
        /// Updates the response asynchronously 
        /// </summary>
        /// <returns>An awaitable task</returns>
        Task UpdateAsync();
    }
}