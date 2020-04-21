import csv #for reading a csv file
i = 0 #to iterate a 2D array for row in reader
k = 0
y = 1
pref = []
pref_tutor = []
student = []
audit = open("Allocation_Log.txt", 'w')
#pathfile = open("path.txt", 'r')
#path = pathfile.read()
csv_file = open("students.csv", 'r') #CSV file being read from
csv_test = open("SMPResult.csv", 'w') #CSV file to write the results to.
writer = csv.writer(csv_test, delimiter=",")
reader = csv.reader(csv_file)
for row in reader: #Each element with the csv file is read and saved below:
    student.append([]) # Adding a new 2D element with each iteration.
    name = row[1]
    ID = row[3]
    pref = row[5]
    for st in student: 
        if(any(ID in st for st in student)):
            student[i-y].append(pref)
            y+=1
            break;
        if(any(ID not in st for st in student)):
            y=1
            student[i].append(ID)
            student[i].append(name)
            student[i].append(pref)
            break;
        else:
            print("do nothing")
    i += 1

student = [x for x in student if x!= []]

for i in range(0, len(student)):
    student[i].append([])

tutor = []
tutors_file = open("Tutors.csv", 'r') #CSV file being read from
tread = csv.reader(tutors_file)
for row in tread:
    tutor.append([])
    name = row[0]
    area = row[1]
    #print(name)
    quota = int(row[2])
    is_free = row[3]
    tutor[k].append(name)
    tutor[k].append(area)
    tutor[k].append(quota)
    tutor[k].append(is_free) 
    k += 1




def get_name_of_preferred_tutor(stud): #Retrieve first preference of student.
    for i in range(0, len(student)):
        if student[i][0] == stud:
            return student[i][2]


def area_is_assigned(area): #check if quota has been met yet
    for i in range(0, len(tutor)):
        if tutor[i][1] == area and tutor[i][2] == 0:
             return True
    return False


def avg1(candidate1): #Retrieve the average mark of the current student the tutor is assigned to
    for x in range(0, len(student)):
        if student[x][0] == candidate1:
            return int(student[x][3])
                

def avg2(candidate2): #Retrieve the average mark of the new student to check if they get their first preference
    for x in range(0, len(student)):
        if student[x][0] == candidate2:
            return int(student[x][3])

def awarded_to(candidate): #Check Tutor Availability
     for x in range(0, len(student)):
         if student[x][0] == candidate: #new student gets their second preference and so on..
            audit.write("\n%s: Checking tutor availbility \n" %(student[x][1]))
            audit.write("%s Preference 1 NOT AVAILABLE \n" %(student[x][2]))
            for i in range(3, 7):
                if not area_is_assigned(student[x][i]):
                    audit.write("%s Preference %s AVAILABLE \n" %(student[x][i], i-1))
                    student[x][8] = student[x][i]
                    return student[x][i]
                else:
                    audit.write("%s Preference %s NOT AVAILABLE \n" %(student[x][i], i-1))
                

def assign(stud, tut, is_First): #Assigning tutor to student
    if is_First:
        for i in range(0, len(student)):
            if student[i][0] == stud:
                audit.write("\n%s: Checking tutor availbility \n" %(student[i][1]))
                student[i][8] = tut
                audit.write("%s Preference 1 AVAILABLE \n" %(student[i][2]))
        for k in range(0, len(tutor)): #Marking the allocated tutor as no longer free
            if tutor[k][1] == tut:
                tutor[k][2] -= 1
                tutor[k][3] = False
    if not is_First:
        for i in range(0, len(student)):
            if student[i][0] == stud:
                student[i][8] = tut
        for k in range(0, len(tutor)): #Marking the allocated tutor as no longer free
            if tutor[k][1] == tut:
                tutor[k][2] -= 1
                tutor[k][3] = False

def main():        
    for x in range(0, len(student)):
        stud = student[x][0] #Get student name
        #if not is_assigned(stud): #check if student is assigned
        areaforFYP = get_name_of_preferred_tutor(stud) #if not get first preference tutor.
        if area_is_assigned(areaforFYP): #if their first preference is assigned:
            assignedArea = awarded_to(stud)#Check availabilities
            assign(stud, assignedArea, False) #Assign the student with the resultant area
        else:
            assign(stud, areaforFYP, True)#if the tutor is still available, simply assign.

def end(): #Basic print implementation for test data
    print("Resolution:\n")
    for i in range(0, len(student)):
        stud = student[i][1]
        area = student[i][8]

        print(stud + " is assigned to " + area)
        returntoweb(stud, area, i)
    

def returntoweb(stud, area, i):
    writer.writerow([stud + " is assigned to " + area])
    
        
   


main() #Execute methods main and end
end()
csv_test.close();
audit.close();
    
  



#Read arrays of data from csv file
#Execute Simulated SMP
#Write result to CSV file.

#To-do:
#1. Write resultant student and tutor to csv file
#2. Read the csv result into a php file



