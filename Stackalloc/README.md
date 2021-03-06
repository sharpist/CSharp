# stackalloc
_________________________________________________________________________________________
#### ```stackalloc``` ������������ � ��������� ������������� ���� ��� ��������� ����� ������ �����. ####
��� �������� ����� ��������� ������ � ��������������� ��������� ����������:
```c#
int* block = stackalloc int[100];
```
������� � ������ C# 7.3 ([C# 7.3](https://github.com/sharpist/C_Sharp/tree/master/7.3#c-73)) ��� �������� ```stackalloc``` ����� ������������ ���������
�������������� �������:
```c#
int* first = stackalloc int[3] { 1, 2, 3 };
int* second = stackalloc int[] { 1, 2, 3 };
int* third = stackalloc[] { 1, 2, 3 };

// ������� ����� ����� 1 ��� � ������ ��������
int* mask = stackalloc[] {
    0b_0000_0000_0000_0001,
    0b_0000_0000_0000_0010,
    0b_0000_0000_0000_0100,
    0b_0000_0000_0000_1000,
    0b_0000_0000_0001_0000,
    0b_0000_0000_0010_0000,
    0b_0000_0000_0100_0000,
    0b_0000_0000_1000_0000,
    0b_0000_0001_0000_0000,
    0b_0000_0010_0000_0000,
    0b_0000_0100_0000_0000,
    0b_0000_1000_0000_0000,
    0b_0001_0000_0000_0000,
    0b_0010_0000_0000_0000,
    0b_0100_0000_0000_0000,
    0b_1000_0000_0000_0000
};
// 1
// 2
// 4
// 8
...
// 4096
// 8192
// 16384
// 32768
```
��� ��� �������� ���� ���������� ([���� ����������](https://github.com/sharpist/C_Sharp/tree/master/Pointer#����-����������)), ```stackalloc``` ������� ������������� ���������.
������������ ��� ����� ���������, ��� ���������� �������. ��� ������������� ```stackalloc```
� ����� CLR ������������� ���������� ����������� ������������ ������.
_________________________________________________________________________________________

#### ������: ����������� ������ 20 ����� ������������������ ��������� ####

� ���� ���� ���� ������, ������ �������� ��������� ��������� 20 ��������� ���� ```int```,
���������� �� ���� (������������ ������), � �����.

����� ����� �������� � ��������� ```fib```.

��� ������ �� ������������ ������ ������ � � �� ����� ���������� � ������� ��������
```fixed```.
����� ������������� ����� ������ �������������� �������� ������������� ������, �������
��� ����������.

![screen capture 1](01.png)
