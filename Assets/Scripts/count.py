import os
count = 0
for filename in os.listdir(os.getcwd()):
    if filename.endswith('.cs'):
        with open(os.path.join(os.getcwd(), filename), 'r') as f: # open in readonly mode
            count += len(f.readlines())

print(count)

#comand
#pret imperf
#vocab