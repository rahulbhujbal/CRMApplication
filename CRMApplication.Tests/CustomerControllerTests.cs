using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CRMApplication.Controllers;
using Moq;
using CRMApplication.Repositories;
using System.Web.Mvc;
using System.Collections.Generic;
using CRMApplication.Models;
using System.Net;
using System.Web.Routing;

namespace CRMApplication.Tests
{
    [TestClass]
    public class CustomerControllerTests
    {
        #region Index
        [TestMethod]
        public void Index_should_return_viewresult()
        {
            // Arrange
            var mockDataRepository = new Mock<IDataRepository>();

            CustomerController sut = new CustomerController(mockDataRepository.Object);

            // Act
            var result = sut.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof( ViewResult));         
            Assert.AreEqual(((ViewResult)result).ViewName, "");
        }

        [TestMethod]
        public void Index_should_return_viewdata()
        {
            // Arrange
            var mockDataRepository = new Mock<IDataRepository>();

            List<CustomerModel> customerList = new List<CustomerModel>()
            {
                new CustomerModel()
                {
                    CustomerId=1
                },
                new CustomerModel()
                {
                    CustomerId=2
                }
            };
            mockDataRepository.Setup(cr => cr.GetAllCustomers()).Returns(customerList);

            CustomerController sut = new CustomerController(mockDataRepository.Object);

            // Act
            var result = (ViewResult)sut.Index();

            Assert.IsInstanceOfType(result.ViewData.Model, typeof(List<CustomerModel> ));

            List<CustomerModel> custList = result.ViewData.Model as List<CustomerModel>;
            Assert.AreEqual(2, custList.Count);
        }
        #endregion Index

        #region Details

        [TestMethod]
        public void Details_should_return_excepton()
        {
            // Arrange
            var mockDataRepository = new Mock<IDataRepository>();

            CustomerController sut = new CustomerController(mockDataRepository.Object);

            // Act
            var result = sut.Details(-1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(HttpNotFoundResult));
        }

        [TestMethod]
        public void Details_should_return_viewresult()
        {
            // Arrange
            var mockDataRepository = new Mock<IDataRepository>();

            mockDataRepository.Setup(x => x.GetCustomer(99)).Returns(new CustomerModel() { FirstName="Abc"});
            CustomerController sut = new CustomerController(mockDataRepository.Object);

            // Act
            var result = sut.Details(99) as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(CustomerModel));
            CustomerModel cust = result.ViewData.Model as CustomerModel;

            Assert.AreEqual(cust.FirstName, "Abc");
        }

        #endregion Details

        #region Select

        [TestMethod]
        public void Select_should_return_viewresult()
        {
            // Arrange
            var mockDataRepository = new Mock<IDataRepository>();

            mockDataRepository.Setup(x => x.GetCustomerWithNotes(99)).Returns(new CustomerModel() { FirstName = "Abc", Notes = new List<NoteModel>() { new NoteModel() {}, new NoteModel() {} } });
            CustomerController sut = new CustomerController(mockDataRepository.Object);

            // Act
            var result = sut.Select(99) as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(List<NoteModel>));
            List<NoteModel> notes = result.ViewData.Model as List<NoteModel>;

            Assert.AreEqual(notes.Count, 2);
        }

        #endregion Select

        #region Create

        [TestMethod]
        public void Create_should_redirect_to_Index()
        {
            // Arrange
            var mockDataRepository = new Mock<IDataRepository>();

            CustomerController sut = new CustomerController(mockDataRepository.Object);

            // Act
            var result = sut.Create(new CustomerModel()) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual(result.RouteValues["Action"], "Index");

        }

        [TestMethod]
        public void Create_should_return_view()
        {
            // Arrange
            var mockDataRepository = new Mock<IDataRepository>();

            CustomerController sut = new CustomerController(mockDataRepository.Object);
            sut.ModelState.AddModelError("", "Error");

            // Act
            var result = sut.Create(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(((ViewResult)result).ViewName, "");
        }
        #endregion Create

        #region Delete

        [TestMethod]
        public void Delete_should_return_viewresult()
        {
            // Arrange
            var mockDataRepository = new Mock<IDataRepository>();

            mockDataRepository.Setup(x => x.GetCustomer(99)).Returns(new CustomerModel() { FirstName = "Abc" });
            CustomerController sut = new CustomerController(mockDataRepository.Object);

            // Act
            var result = sut.Delete(99) as ViewResult;

            // Assert
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(CustomerModel));
            CustomerModel cust = result.ViewData.Model as CustomerModel;

            Assert.AreEqual(cust.FirstName, "Abc");
        }

        [TestMethod]
        public void Delete_should_redirect_to_Index()
        {
            // Arrange
            var mockDataRepository = new Mock<IDataRepository>();

            CustomerController sut = new CustomerController(mockDataRepository.Object);

            // Act
            var result = sut.DeleteConfirmed(99) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual(result.RouteValues["Action"], "Index");

        }

        #endregion Delete
    }
}
