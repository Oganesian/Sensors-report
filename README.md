# Sensors-report

A program for generating reports based on data read from sensors. 
The data is stored in the database. 
The program allows you to:
* add
* change
* delete new data in the database
* create reports based on data in the form of Word documents

## Database — «sensors_info»
### Table — «sensors_data»
```
number_of_sensor — VARCHAR(32), PRIMARY_KEY
name_of_sensor — VARCHAR(64)
type_of_sensor — VARCHAR(64)
value_a	— DOUBLE
value_b	— DOUBLE
value_c	— DOUBLE
value_d	— DOUBLE
zero_value — DOUBLE
expiration_date	— DATE
```
## Frameworks used
* [MahApps.Metro](https://github.com/MahApps/MahApps.Metro) — UI toolkit for WPF
* [MySQL.Data](https://dev.mysql.com/downloads/connector/net/6.10.html) — MySQL/NET Connector
* [OpenXML SDK 2.5](https://msdn.microsoft.com/en-us/library/office/bb448854.aspx) — The strongly-typed classes for use with Open XML documents.