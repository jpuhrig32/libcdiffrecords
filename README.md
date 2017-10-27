# libcdiffrecords
A library and test applications for manipulation of clostridium difficile surveillance data in the hospital, with a focus on
Froedtert Memorial Lutheran Hospital in Milwaukee WI.

The goal is basically to have a library of available datatypes for the manipulation of surveillance data, filtering based on temporal factors, and creating reports for either basic epidemiology, or to pick interesting samples in our storage freezer for further analysis.

The library consists of a few core datatypes - Bin, Admission, DataPoint, Tube, and StorageData.

DataPoint - a single line in the surveillance data that contains information on the patient who gave the sample, admission dates, sample dates, locations, and is expandable to handle future fields. This is the atomic unit of analysis.

Admission - A grouping of DataPoints that occurs within a single healthcare encounter. The Admission also includes calculated fields for things like, outcome (remained negative, positive on admission, missing admission samples....) This is the most datatype used for filters and analysis. Admissions are rarely created directly, but rather obtained from Bins, which will automatically create Admissions when DataPoints are added.

Bin - A self-organizing datatype that takes in both DataPoints and Admissions, and will organize them into 4 tables - Data (List of DataPoints), DataByPatient ( A Dictionary keyed by patient medical record number (MRN) with a list of all samples for that patient)), DataByAdmissionTable (A Dictionary, keyed by patient MRN, containing a list of Admissions for that patient). Bins also contain several calculated fields, such as %Female, Age Range, Median Age, Number of unique patients. This makes it useful for performing summary statistics.

Filtering Operations:
Filtering is handled by static methods of the class DataFilter. DataFilter has 3 basic operation types, all of which typically take
Bin datatypes as arguments, and return Bin datatypes as results. Filters are meant to stack - chaining together multiple filters can allow for more complex methods of data selection.

Operation Types:
1. Filter - Filter refers to keeping a particular set of data, so things like FilterAdmissionsWithAdmissionSamples() would pick all samples that are part of admissions with an early sample taken.

2. Remove - Basically the opposite of Filter. Rather than keeping samples meeting criteria, it removes them.

3. Stratify - Splits a Bin into multiple ones based on criteria, such as units or age groups. This is usually the final step before feeding the multiple Bins into reporting functions.

It is critical to note that the order of operations with filtering functions does actually matter. The general rule of thumb is to start with broader filtering criteria, then narrowing down further with each subsequent filter step. For example, if we wanted to find stool samples for all patients who were negative on admission, then turned positive, the first filter would be to filter for admission outcome (negative on admission, turned positive), then filter for stool samples, then if desired, filter for surveillance stool samples. Doing those filters out of order, say filtering for stools first, then by admission outcome, would likely result in losing perfectly valid admissions, because for some patients, their admission sample might have been a swab. 

The calculated fields in Bins and Admissions are updated everytime data from them is pulled, which means, that, a filtering operation IS NOT going to preserve these previous values - making that order of operations that much more critical. 

The last operations should be selecting samples from the storage database, and Stratification operations (Stratifications can be done at any point, but then one needs to make sure to apply filters to each of the split bins produced by the stratification operation).


It should be noted that Bins themselves have 4 methods that can be used to extend or improvise new filters:
1. Addition / Union - Adds the contents of two Bins together (Bins have built-in deduplication, so no worries on pre-processing)
   Can use the + operator for this.
2. Intersection - Returns the samples that are common to both bins.
3. Subtraction / Exclusion - Returns the contents that are unique to the first bin.
   Can use the - (subtraction) operator for this.
4. Complement - Returns the items that are unique to one bin or the other, but not both (Almost like an XOR type operation).

Generating Reports:
Currently, there are only a few report types available:
MasterReport - which is a combination demographic / surveillance data report for a given Bin
NAATReport - A report on which patients have undergone subsequent clinical testing for a given bin.






