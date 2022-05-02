
#%% 
# Python Basics
print("Hello")

a = 5
b = 3
c = a * b

print(c)

print(type(a)) # sagt welcher Datentyp das ist

varA = "123"
varB = 456

print(varA + str(varB))

myList = [1, 2, 3]
print(type(myList))
myOtherList = ["Bart", "Frank", "Bob"]
myMixedList = ["hello", 345, 34.567]

len(myList)

print(myOtherList[0])

# For each Schleifen
for name in myOtherList:
    print(name)

# Eine Range anlegen
series = range(1, 10)

# For Schleife
for i in range(0, 10):
    print(i)

# In Liste suchen
if "Bart" in myOtherList:
    print("Yes it is")

a = 5
b = 10
# If Else
if a < b:
    print("Numbers are nice")
else:
    print("Numbers not good")

# Funktionen
def addNumbers(a, b):
    return a + b

print(addNumbers(a, b))
