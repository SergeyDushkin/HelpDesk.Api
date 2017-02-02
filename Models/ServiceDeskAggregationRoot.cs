using System;
using System.Collections.Generic;

namespace servicedesk.api
{
    public class ServiceDeskAggregationRoot
    {
        public class Test
        {
            public Test()
            {
                var ticket = Ticket.Create();
                
                ticket.Apply();

                ticket.CreateWork();
            }
        }

        public class Ticket
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public Company Client { get; set; }
            public Address Address { get; set; }
            public Contract Contract { get; set; }
            public Priority Priority { get; set; }
            public DateTime Deadline { get; set; }
            public String Description { get; set; }
            public Status Status { get; set; }
            public Service Service { get; set; }
            public Operator Operator { get; set; }
            public ResponsibleLine ResponsibleLine { get; set; }

            public List<Work> Works { get; set; }

            public static Ticket Create()
            {
                throw new NotImplementedException();
            }

            public Ticket Apply()
            {
                throw new NotImplementedException();
            }

            public Ticket CreateWork()
            {
                throw new NotImplementedException();
            }

        }

        public class Work
        {
            public Company Supplier { get; set; }
            public Worker Worker { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public String Description { get; set; }
            public WorkStatus Status { get; set; }
            public List<File> Files { get; set; }

            public Work AddFile()
            {
                throw new NotImplementedException();
            }
        }

        /*
        public class WorkTeam
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public Worker Worker { get; set; }
            public Car Car { get; set; }

            public WorkTeamStatus Status { get; set; }
        }*/

        public class Company
        {
            public string Name { get; set; }
        }

        public class Address
        {
            public string Name { get; set; }
        }

        public class Contract
        {
            public string Name { get; set; }

            public string Number { get; set; }
            public DateTime Date { get; set; }

            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }

            public Company Client { get; set; }

            public Address[] AllowedAddresses { get; set; }
            public Service[] AllowedServices { get; set; }
        }

        public class User
        {
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public string GenderCode { get; set; }
            public DateTime DateOfBirth { get; set; }
            public DateTime DateOfDeath { get; set; }
        }

        public class Priority
        {
            public string Name { get; set; }
        }

        public class Status
        {
            public string Name { get; set; }
        }

        public class WorkStatus
        {
            public string Name { get; set; }
        }

        public class WorkTeamStatus
        {
            public string Name { get; set; }
        }

        public class Employee : User
        {
            public string Name { get; set; }
            public string PersonnelNumber { get; set; }
        }

        public class Operator : Employee
        {
        }

        public class Worker : Employee
        {
        }

        public class Service
        {
            public string Name { get; set; }
        }

        public class Car
        {
            public string Name { get; set; }

            public int Year { get; set; }
            public string Make { get; set; }
            public string Model { get; set; }
            public string Number { get; set; }
        }

        public class Schedule
        {
            public Employee Employee { get; set; }
            public DateTime Date { get; set; }
            public decimal StartTime { get; set; }
            public decimal EndTime { get; set; }
        }

        public class ResponsibleLine
        {
            public string Name { get; set; }
        }

        public class File
        {
            public string Name { get; set; }
        }
    }
}


/*
 * ������
- ����/�����
- �������� ���������
- ������ ���������
- ������� => ��������� + ���� ���������� �� ��������� ��������
- ����� ������
- ������ ������
- ��������
- ����� ������������

������
- ������
- ������ (�������� ������ ���, �������-���������������� ������)
- ����/����� ������ �����
- ����/����� ��������� �����
- �������� �����
- ����������� � ����������

��������
	- ���
	- ���
	- �����
	- �����

����������
	- �������
	- ��� �����
	- ������ ������ (���������� ���/����, �������� 5/2 10-19)
	- ��������� (��������� �� ���, ��������� � �� = ���������������� ���������)

���������� � �������� ������������
	- ��������� ������������ (��� ����)
	- ��������� ��������

������� �� ��
- ����
- ����������
- ���� ��������
- ����������� ���������������� �������
- �����
- ������ �������� �� ��������
- ������(�������) �� �������� + ���������� (��� � ����, ����� ������ ����� ������������) + ��������� (� ���������� ���� ����������� ����� �������) + ����
 * */
