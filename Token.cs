using System;

// Базовый абстрактный класс для всех токенов
public abstract class Token
{
    // Абстрактное свойство, которое должны реализовать все наследники
    public abstract string Value { get; }

    // Виртуальный метод, который можно переопределить в наследниках
    public virtual int Length => Value.Length;

    // Абстрактный метод для сравнения токенов
    public abstract override bool Equals(object obj);

    // Виртуальный метод ToString - по умолчанию возвращает Value
    public override string ToString() => Value;
}
