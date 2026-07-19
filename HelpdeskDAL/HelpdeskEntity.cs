/**
 * File name:	    HelpdeskEntity.cs
 * Purpose: 		Serves as a base class for all Helpdesk database entities, 
 *                          providing a common Id property and a concurrency control Timer.
 * Author:			Milana Meftakhutdinova
 * Date: 			November 5, 2025
 */

using System.ComponentModel.DataAnnotations;

namespace HelpdeskDAL
{
    public class HelpdeskEntity
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[]? Timer { get; set; }
    }
}
