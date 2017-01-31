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
 * Заявки
- Дата/Время
- Компания заявитель
- Объект заявитель
- Договор => Приоритет + Срок исполнения на основании договора
- Текст Заявки
- Статус заявки
- Оператор
- Линия исполнителей

Работы
- Заявка
- Услуга (например Ремонт ТРК, Планово-Профилактические работы)
- Дата/Время начала работ
- Дата/Время окончания работ
- Описание работ
- Исполнители и Автомобили
 * */
