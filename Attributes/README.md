# ��������
_________________________________________________________________________________________
#### �������� ����� ��������� �����������: ####

* ��������� � ��������� ���������� (�������� � �����, ������������ � ���������), �����
���������������� �������� ����� ������� ����� �������������� ����������.

* ���� ��� ��������� ��������� ��������� �� ���� ������, � ������ ��� � ����� ������
��������� (�������, ���������...).

* ����� ��������� ���������, ��� �� ��� ������ � ��������.

* ����������� ��������� ��������� ��������� ����������� ���������� ��� ���������� �
������ ����������.

�������� ����� ������������ ����� � ����� ����������� (����� ���������� ���� ����������
����������).
```c#
[Serializable]
public class SampleClass
{
    // ������� ����� ���� ����� ���� �������������
}
```

��� ����� ��������� ������������� ��������� Attribute, �� ��� ������������� �������� �
���� ���� ������� ����� �� ��������� � ```[DllImport]``` ������������ ```[DllImportAttribute]```.

������ �������� ����� �����������, �������������� ��� ����������� ���������:

1. ����������� ��������� �� ����� ���������� � ������ ����������� �������.

2. ����������� ��������� �������� ���������������, ����� ����������� � ����� �������.
```c#
[DllImport("user32.dll")]
[DllImport("user32.dll", SetLastError=false, ExactSpelling=false)]
[DllImport("user32.dll", ExactSpelling=false)]
```

������� ������ �������� � ��� ��������, � ������� ����������� �������. �� ���������
������� ����������� � ���� ��������, ����� ������� �� ������.
����� ����� ������� ���������� ������� ������ ��������:
```c#
[target : attribute-list]
```

��� ```target```:

* ```assembly``` � ��� ������.

* ```module``` � ������ ������� ������.

* ```field```* � ���� � ������ ��� ���������.

* ```event``` � �������.

* ```method``` � �����, ���� ������ ������� � ��������� ```get``` � ```set```.

* ```param``` � ��������� ������ ��� ��������� ������ ������� ```set```.

* ```property``` � ��������.

* ```return``` � ������������ �������� ������, ����������� �������� ��� ������ ������� �
��������� ```get```.

* ```type``` � ���������, �����, ���������, ������������ ��� �������.

*������� �������� ```field``` ������������� ������� ��������� ���������� �������������
������������ ��������

```c#
// default: ����������� � ������
[ValidatedContract]
int Method1() { return 0; }

// ����������� � ������
[method: ValidatedContract]
int Method2() { return 0; }

// ����������� � ������������� ��������
[return: ValidatedContract]
int Method3() { return 0; }
```

��� ����������� �� ������� �������� ```AttributeUsage```, ��� ������� ��������� �������,
���������� ���� ������ ������� ������ ��� ```return```.
(���������� �� ���������� �������� ```AttributeUsage``` ��� ���������� ����������
���������������).
_________________________________________________________________________________________
#### ���������� �������� ####

���������� �������� � ����������� �� ���� ������ ��� ������. ��������, �������
```AssemblyVersionAttribute``` ����� ������������ ��� ����������� �������� � ������ � ������:
```c#
[assembly: AssemblyVersion("1.0.0.0")]
```
���������� �������� ������������ � �������� ���� ����� ����� �������� ```using``` ��������
������ � ����� ����� ������������ �����.

� �������� C# ���������� �������� ���������� � ���� ```AssemblyInfo.cs```.


#### �������� ������ � ��� ��������, ������� ������������� �������� � ������: ####

1. �������� ������������� ������.

2. �������������� ��������.

3. �������� ��������� ������.

�������� ������������� ������ ���������� ������������� ������ (���, ������, ���� �
������������ ���������), ��������� ������ ��� ������ � �������� ������������� ��� ������
�� �� � ����.

 � ```AssemblyName``` ��������� ��������� ������������� ������.

 � ```AssemblyVersionAttribute``` ������ ������ ������.

 � ```AssemblyCultureAttribute``` ���������, ����� ���� � ������������ ��������� ������������
������.

 � ```AssemblyFlagsAttribute``` ���������, ������������ �� ������ ������������ ���������� �� �����
����������, � ����� �������� ��� � ����� ������ ����������.

�������������� �������� ����� ������������ ��� �������������� �������������� �������� �
�������� ��� �������� � ������.

 � ```AssemblyProductAttribute``` ���������� ������������� �������, �������� ��� �������� ���
��������� ������.

 � ```AssemblyTrademarkAttribute``` ���������� ������������� �������, �������� �������� ���� ���
��������� ������.

 � ```AssemblyInformationalVersionAttribute``` ���������� ������������� �������, ��������
�������������� ������ ��� ��������� ������.

 � ```AssemblyCompanyAttribute``` ���������� ������������� �������, �������� �������� �����������
��� ��������� ������.

 � ```AssemblyCopyrightAttribute``` ���������� ������������� �������, �������� ����������� ��
��������� ������ ��� ��������� ������.

 � ```AssemblyFileVersionAttribute``` ���� ����������� �������� ������������ ������������ �����
������ ��� ������� ������ ����� Win32.

 � ```CLSCompliantAttribute``` ���������, ������������� �� ������ ������������ CLS.

�������� ��������� ������ ����� ������������ ��� �������������� �������� � ���������
������ (���������, ��������, ��������� �� ��������� � ������������).

 � ```AssemblyTitleAttribute``` ���������� ������������� �������, �������� �������� ������ ���
��������� ������.

 � ```AssemblyDescriptionAttribute``` ���������� ������������� �������, �������� �������� ������
��� ��������� ������.

 � ```AssemblyConfigurationAttribute``` ���������� ������������� �������, �������� ������������
������ ��� ��������� ������.

 � ```AssemblyDefaultAliasAttribute``` ���������� �������� ��������� �� ��������� ��� ���������
������.
_________________________________________________________________________________________
#### �������� ������������� ��������� ####

��������, ��������� �������� ��� ����� � ������ ������������, ��� ����� ����� ����������
����� ������������� ��������� ```Author```:

1. ������������ ����� ��������, ����������� �� ```System.Attribute```, ��� ��� ������ � ���
��� �������� ```Author```.

a. ��������� ������������ �������� ������������ ����������� �������������� ��������
(� ������ ������ ```name``` ����������� ��������).

b. ��� �������� ���� ��� ��������, ��������� ��� ������ � ������, �������� ������������
����������� (� ������ ������ ```version``` ����������� ��������).

2. ������������ ������� ```AttributeUsage```, �������� ������� ```Author``` ���������� ������ ���
������, ���������� ```struct``` � ��������� ������� ������.

a. ```AttributeUsage``` ����� ����������� �������� ```AllowMultiple```, ������������ ���
�������������� �������� ����������� ��� ������������ �������������.

![screen capture 1](01.png)

_________________________________________________________________________________________
#### ��������� � ��������� � ������� ��������� ####

��������� ��������� ��������� ��������, ����������� � �������������� ����������.
����� ```GetCustomAttributes``` ���������� ������ ��������, ���������� ������������� ���������
��������� ���� �� ����� ����������.

� �������, �������������� ����, ������������ �������� ```Author```
```c#
[Author("R. Koch", version = 1.3)]  
public class SampleClass
```
������������ ���������� ����:
```c#
Author anonymousAuthorObject = new Author("R. Koch");  
anonymousAuthorObject.version = 1.3;
```
������, ��� �� ����������� �� ��� ���, ���� � ```SampleClass``` �� ����� ��������� ��������.

#### ����� �������, ����� ������ ```GetCustomAttributes``` � ```SampleClass``` �������� � ����, ��� ������ ####
#### ```Author``` ����� ������ � ��������������� ���, ��� �������� ����. ####
����� ```GetCustomAttributes``` ���������� ������ ```Author``` � ��� ������ ������� ��������� �
�������.

![screen capture 2](02.png)

*����������� �������� ������ ���������� � ������� ������������ ���, ����� ������
��������� ������������ ��� � ������������ ������� ��������


������������ ������ ```Type```, ����� �������� ������ �� �������� �� ������ ������.

#### ����� ����� ������������ ����� ������ ```Type```, ����� �������� ��������� ������ � ����� ####
#### ����������� ������: ####

��� ����� ����������� ����� ```Type.GetMethods```, ���������� ���������� ���� ������� � ������
�������� ```System.Reflection.MemberInfo``` � ���������� ������ �� �������� �� ������ �������.
����������� ������� ����� ����� �������� �� ������ ������� ��������� �����
```Type.GetProperties```.
_________________________________________________________________________________________
