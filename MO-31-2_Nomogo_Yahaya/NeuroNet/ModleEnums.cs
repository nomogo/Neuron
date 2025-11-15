

namespace MO_31_2_Nomogo_Yahaya.NeuroNet
{
    enum MemoryMode
    {
        GET, // считывание памяти
        SET, // сохранение памяти
        INIT // инициализация памяти
    }
    enum NeuronType
    {
        Hidden, //скрытый
        Output //выходной
    }
    enum NetworkMode
    {
        Train, //обучение
        Test, //проверка
        Demo //распознавание
    }
}
