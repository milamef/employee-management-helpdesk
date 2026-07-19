/**
 * File name:	    UpdateStatus.cs
 * Purpose: 		Represents the possible outcomes of an update operation in the Helpdesk repository.
 * Author:			Milana Meftakhutdinova
 * Date: 			November 5, 2025
 */

namespace HelpdeskDAL
{
    public enum UpdateStatus
    {
        Ok = 1,
        Failed = -1,
        Stale = -2
    }
}
