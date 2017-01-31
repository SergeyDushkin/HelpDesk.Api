using System;

namespace servicedesk.api
{
    public class ServiceDeskAggregationRoot
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public class Ticket
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
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
 * */
