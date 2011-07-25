import urllib
import urllib2
import os
import time
import sys
import traceback
from xml.dom import minidom
from time import gmtime, strftime

POLL_TIME_IN_SECONDS = 30
FLASH_TIME_IN_SECONDS = 5
SYNDICATION_URLS = ["https://TEAMCITY-URL/guestAuth/feed.html?buildTypeId=bt122&itemsType=builds&userKey=guest"]

lastColor = ""

def currentTime():
  return strftime("%Y-%m-%d %H:%M:%S", gmtime())

def showColor(color):
  global lastColor
  if color != lastColor:
    os.system("cmd /c buildlight.exe %s Flash" % color)
    time.sleep(FLASH_TIME_IN_SECONDS)
    os.system("cmd /c buildlight.exe %s On" % color)
  lastColor = color

def wasSuccessful(dom):
  for entry in dom.getElementsByTagName('entry'):
    title = entry.getElementsByTagName('title')[0].firstChild.data
    print "[%s] %s" % (currentTime(), title)
    return title.endswith("was successful")

def getAllStatuses():
  doms = [minidom.parse(urllib2.urlopen(url)) for url in SYNDICATION_URLS]
  statuses = [wasSuccessful(dom) for dom in doms]
  return statuses

def setLight(statuses):
  if False in statuses:
    color = "Red"
  else:
    color = "Green"
  showColor(color)


while True:
  try:
    statuses = getAllStatuses()
    setLight(statuses)
  except:
    exceptionType, value = sys.exc_info()[:2]
    print "[%s] Unexpected exception: %s - %s" % (currentTime(), exceptionType, value)
    traceback.print_exc(file=sys.stderr)
    showColor("Yellow")

  time.sleep(POLL_TIME_IN_SECONDS)

