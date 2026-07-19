/**
 * File name:	    ViewModelTests.cs
 * Purpose: 		Contains unit tests to verify the functionality of methods in the EmployeeViewModel class.
 * Author:			Milana Meftakhutdinova
 * Date: 			November 5, 2025
 */

using HelpdeskDAL;
using HelpdeskViewModels;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Xunit.Abstractions;

namespace CasestudyTests
{
    public class ViewModelTests
    {
        private readonly ITestOutputHelper output;
        public ViewModelTests(ITestOutputHelper output)
        {
            this.output = output;
        }


        [Fact]
        public async Task Employee_GetByEmailTest()
        {
            EmployeeViewModel vm = new() { Email = "bs@abc.com" };
            await vm.GetByEmail();
            Assert.NotNull(vm.Firstname);
        }


        [Fact]
        public async Task Employee_GetByIdTest()
        {
            EmployeeViewModel vm = new() { Id = 10 };
            await vm.GetById();
            Assert.NotNull(vm.Firstname);
        }


        [Fact]
        public async Task Employee_GetAllTest()
        {
            List<EmployeeViewModel> allEmployeeVms;
            EmployeeViewModel vm = new();
            allEmployeeVms = await vm.GetAll();
            Assert.True(allEmployeeVms.Count > 0);
        }


        [Fact]
        public async Task Employee_AddTest()
        {
            EmployeeViewModel vm;
            vm = new()
            {
                Title = "Ms.",
                Firstname = "Milana",
                Lastname = "Meftakhutdinova",
                Email = "mm@fan.com",
                Phoneno = "(777) 777-7777",
                DepartmentId = 100
            };
            await vm.Add();
            Assert.True(vm.Id > 0);
        }


        [Fact]
        public async Task Employee_UpdateTest()
        {
            EmployeeViewModel vm = new() { Phoneno = "(777) 777-7777" };
            await vm.GetByPhoneNumber();
            vm.Email = vm.Email == "mm@fan.com" ? "milana.meftakhutdinova@gmail.com" : "mm@fan.com";
            Assert.True(await vm.Update() == 1);
        }


        [Fact]
        public async Task Employee_DeleteTest()
        {
            EmployeeViewModel vm = new() { Email = "milana.meftakhutdinova@gmail.com" };
            await vm.GetByEmail();
            Assert.True(await vm.Delete() == 1);
        }


        [Fact]
        public async Task Employee_GetByPhoneNumberTest()
        {
            EmployeeViewModel vm = new() { Phoneno = "(555) 555-5555" };
            await vm.GetByPhoneNumber();
            Assert.NotNull(vm.Phoneno);
        }


        [Fact]
        public async Task Employee_ComprehensiveVMTest()
        {
            EmployeeViewModel evm = new()
            {
                Title = "Mr.",
                Firstname = "Some",
                Lastname = "Employee",
                Email = "some@abc.com",
                Phoneno = "(777)777-7777",
                DepartmentId = 100 // ensure department id is in Departments table 
            };
            await evm.Add();
            output.WriteLine("New Employee Added - Id = " + evm.Id);
            int? id = evm.Id;  // need id for delete later 
            await evm.GetById();
            output.WriteLine("New Employee " + id + " Retrieved");
            evm.Phoneno = "(555)555-1233";
            if (await evm.Update() == 1)
            {
                output.WriteLine("Employee " + id + " phone# was updated to - " + evm.Phoneno);
            }
            else
            {
                output.WriteLine("Employee " + id + " phone# was not updated!");
            }

            evm.Phoneno = "Another change that should not work";
            if (await evm.Update() == -2)
            {
                output.WriteLine("Employee " + id + " was not updated due to stale data");
            }

            evm = new EmployeeViewModel
            {
                Id = id
            };
            // need to reset because of concurrency error 
            await evm.GetById();

            if (await evm.Delete() == 1)
            {
                output.WriteLine("Employee " + id + " was deleted!");
            }
            else
            {
                output.WriteLine("Employee " + id + " was not deleted");
            }
            // should throw expected exception 
            Task<NullReferenceException> ex = Assert.ThrowsAsync<NullReferenceException>(async ()=> await evm.GetById());
        }


        [Fact]
        public async Task Call_ComprehensiveVMTest()
        {
            EmployeeViewModel evm = new();
            ProblemViewModel pvm = new();

            EmployeeViewModel problemEmp = new();
            problemEmp.Email = "mm@abc.com";
            await problemEmp.GetByEmail();
            Assert.NotNull(problemEmp);

            EmployeeViewModel techEmp = new();
            techEmp.Email = "bb@abc.com";
            await techEmp.GetByEmail();
            Assert.NotNull(techEmp);

            ProblemViewModel prob = new();
            prob.Description = "Memory Upgrade";
            await prob.GetByDescription();
            Assert.NotNull(prob);

            CallViewModel cvm = new()
            {
                EmployeeId = (int)problemEmp.Id!,
                TechId = (int)techEmp.Id!,
                ProblemId = (int)prob.Id!,
                DateOpened = DateTime.Now,
                DateClosed = null,
                OpenStatus = true,
                Notes = $"{problemEmp.Lastname}’s has bad RAM, {techEmp.Lastname} to fix it"
            };
            await cvm.Add();
            output.WriteLine("New Call Added - Id = " + cvm.Id);
            int id = cvm.Id;  // need id for delete later 
            await cvm.GetById();
            output.WriteLine("New Call " + id + " Retrieved");

            cvm.Notes += "\n Ordered new RAM!"; 

            if (await cvm.Update() == 1)
            {
                output.WriteLine("Call " + id + " was updated to - " + cvm.Notes);
            }
            else
            {
                output.WriteLine("Call " + id + " was not updated!");
            }

            cvm.Notes = "Another change that should not work";
            if (await cvm.Update() == -2)
            {
                output.WriteLine("Call " + id + " was not updated due to stale data");
            }

            cvm = new CallViewModel
            {
                Id = id
            };
            // need to reset because of concurrency error 
            await cvm.GetById();

            if (await cvm.Delete() == 1)
            {
                output.WriteLine("Call " + id + " was deleted!");
            }
            else
            {
                output.WriteLine("Call " + id + " was not deleted");
            }
            // should throw expected exception 
            Task<NullReferenceException> ex = Assert.ThrowsAsync<NullReferenceException>(async () => await cvm.GetById());
        }
    }
}
