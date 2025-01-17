namespace ParkingLotTest
{
    using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
    using ParkingLot;
    using System;
    using System.Collections.Generic;
    using Xunit;

    public class ParkingLotTest
    {
        [Fact]
        public void Should_return_a_parking_ticket_when_parking_boy_parks_a_car_into_parking_lot_given_a_car_and_a_parking_lot()
        {
            //given
            ParkingBoy parkingBoy = new ParkingBoy();
            Car car = new Car("car1");

            //when
            var parkingTicket = parkingBoy.Park(car);

            //then
            Assert.Equal(typeof(ParkingTicket), parkingTicket.GetType());
        }

        [Fact]
        public void Should_remove_car_from_parking_lot_when_parking_boy_fetch_the_car_given_a_parking_ticket()
        {
            //given
            ParkingBoy parkingBoy = new ParkingBoy();
            Car car = new Car("car1");
            var parkingTicket = parkingBoy.Park(car);

            //when
            parkingBoy.FetchCar(parkingTicket);

            //then
            foreach (var parkingLot in parkingBoy.ParkingLots)
            {
                Assert.Null(parkingLot.Cars.Find(car => car.Name == "car1"));
            }
        }

        [Fact]
        public void Should_add_multiple_car_to_parking_lot_when_parking_boy_parks_cars_given_cars_and_a_parking_lot()
        {
            //given
            ParkingBoy parkingBoy = new ParkingBoy();
            List<Car> cars = new List<Car> { new Car("car1"), new Car("car2") };

            //when
            parkingBoy.ParkMultipleCars(cars);

            //then
            Assert.Equal(2, parkingBoy.ParkingLots[0].Cars.Count);
        }

        [Fact]
        public void Should_fetch_right_car_when_parking_boy_parks_multiple_cars_given_a_parking_ticket()
        {
            //given
            ParkingBoy parkingBoy = new ParkingBoy();
            List<Car> cars = new List<Car> { new Car("car1"), new Car("car2") };
            List<ParkingTicket> parkingTickets = parkingBoy.ParkMultipleCars(cars);

            //when
            parkingBoy.FetchCar(parkingTickets[1]);

            //then
            foreach (var parkingLot in parkingBoy.ParkingLots)
            {
                Assert.Null(parkingLot.Cars.Find(car => car.Name == parkingTickets[1].CarName));
            }
        }

        [Fact]
        public void Should_not_fetch_the_car_when_customer_gives_a_wrong_ticket()
        {
            //given
            ParkingBoy parkingBoy = new ParkingBoy();
            Car car = new Car("car1");
            Customer customer = new Customer("customerName", car);
            customer.GiveCarToPark(parkingBoy);
            customer.ParkingTicket = new ParkingTicket("wrong");

            //when
            //then
            Assert.Throws<Exception>(() => customer.GetCar(parkingBoy));
        }

        [Fact]
        public void Should_not_fetch_the_car_when_customer_gives_no_ticket()
        {
            //given
            ParkingBoy parkingBoy = new ParkingBoy();
            Car car = new Car("car1");
            Customer customer = new Customer("customerName", car);
            customer.GiveCarToPark(parkingBoy);
            customer.ParkingTicket = null;

            //when
            //then
            Assert.Throws<Exception>(() => customer.GetCar(parkingBoy));
        }

        [Fact]
        public void Should_not_fetch_the_car_when_customer_gives_used_ticket()
        {
            //given
            ParkingBoy parkingBoy = new ParkingBoy();
            Car car = new Car("car1");
            Customer customer = new Customer("customerName", car);
            customer.GiveCarToPark(parkingBoy);
            customer.ParkingTicket.Use();

            //when
            //then
            Assert.Throws<Exception>(() => customer.GetCar(parkingBoy));
        }

        [Fact]
        public void Should_customer_get_no_ticket_when_parking_lot_is_full()
        {
            //given
            int parkingLotCapacity = 10;

            ParkingBoy parkingBoy = new ParkingBoy();
            Car car = new Car("carToPark");
            Customer customer = new Customer("customerName", car);

            for (int i = 0; i < parkingLotCapacity; i++)
            {
                parkingBoy.Park(new Car("car" + i.ToString()));
            }

            //when
            bool isSuccess = customer.GiveCarToPark(parkingBoy);

            //then
            Assert.False(isSuccess);
            Assert.Null(customer.ParkingTicket);
        }

        [Fact]
        public void Should_not_park_when_parking_boy_park_given_a_parked_car()
        {
            //given
            ParkingBoy parkingBoy = new ParkingBoy();
            Car car = new Car("car1");
            parkingBoy.Park(car);

            //when
            var parkingTicket = parkingBoy.Park(car);

            //then
            Assert.Null(parkingTicket);
        }

        [Fact]
        public void Should_not_park_when_parking_boy_park_given_a_null_car()
        {
            //given
            ParkingBoy parkingBoy = new ParkingBoy();

            //when
            var parkingTicket = parkingBoy.Park(null);

            //then
            Assert.Null(parkingTicket);
        }

        [Fact]
        public void Should_return_error_message_when_customer_get_car_given_a_null_ticket()
        {
            //given
            ParkingBoy parkingBoy = new ParkingBoy();
            Car car = new Car("car1");
            Customer customer = new Customer("customerName", car);
            customer.GiveCarToPark(parkingBoy);
            customer.ParkingTicket = null;

            //when
            //then
            Assert.Throws<Exception>(() => customer.GetCar(parkingBoy));
        }

        [Fact]
        public void Should_return_error_message_Please_provide_your_parking_ticket_when_customer_get_car_given_a_null_ticket()
        {
            //given
            ParkingBoy parkingBoy = new ParkingBoy();
            Car car = new Car("car1");
            Customer customer = new Customer("customerName", car);
            customer.GiveCarToPark(parkingBoy);
            customer.ParkingTicket = null;

            //when
            //then
            var exception = Assert.Throws<Exception>(() => customer.GetCar(parkingBoy));
            Assert.Equal("Please provide your parking ticket.", exception.Message);
        }

        [Fact]
        public void Should_return_error_message_Not_enough_position_when_parking_boy_park_given_parking_lot_no_position()
        {
            //given
            int parkingLotCapacity = 10;

            ParkingBoy parkingBoy = new ParkingBoy();
            Car car = new Car("carToPark");

            for (int i = 0; i < parkingLotCapacity; i++)
            {
                parkingBoy.Park(new Car("car" + i.ToString()));
            }

            //when
            //then
            var exception = Assert.Throws<Exception>(() => parkingBoy.Park(car));
            Assert.Equal("Not enough position.", exception.Message);
        }

        [Fact]
        public void Should_park_car_to_second_parking_lot_when_parking_boy_park_given_first_parking_lot_full()
        {
            //given
            int parkingLotCapacity = 10;

            ParkingBoy parkingBoy = new ParkingBoy();
            Car car = new Car("carToPark");
            parkingBoy.ParkingLots.Add(new ParkingLot());

            for (int i = 0; i < parkingLotCapacity; i++)
            {
                parkingBoy.Park(new Car("car" + i.ToString()));
            }

            //when
            parkingBoy.Park(car);

            //then
            Assert.Single(parkingBoy.ParkingLots[1].Cars);
        }

        [Fact]
        public void Should_park_to_second_parking_lot_when_parking_boy_park_given_first_parking_lot_less_empty_than_second()
        {
            //given
            SmartParkingBoy parkingBoy = new SmartParkingBoy();
            Car car = new Car("carToPark");
            parkingBoy.ParkingLots.Add(new ParkingLot());
            parkingBoy.Park(new Car("car"));

            //when
            parkingBoy.Park(car);

            //then
            Assert.Single(parkingBoy.ParkingLots[1].Cars);
        }
    }
}
